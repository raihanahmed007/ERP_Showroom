using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Hangfire;
using Microsoft.Extensions.Logging;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.fin.WorkflowCommands;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.fin.Workflows;

public class WorkflowOrchestrator
{
    private readonly IMediator _mediator;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ICreditScoreService _creditScoreService;
    private readonly ITesseractOcrService _ocrService;
    private readonly ILogger<WorkflowOrchestrator> _logger;

    public WorkflowOrchestrator(
        IMediator mediator,
        IBackgroundJobClient backgroundJobClient,
        ICreditScoreService creditScoreService,
        ITesseractOcrService ocrService,
        ILogger<WorkflowOrchestrator> logger)
    {
        _mediator = mediator;
        _backgroundJobClient = backgroundJobClient;
        _creditScoreService = creditScoreService;
        _ocrService = ocrService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task StartFromLeadAsync(long leadId)
    {
        _logger.LogInformation("Starting HP Agreement workflow from Lead: {LeadId}", leadId);

        var lead = await _mediator.Send(new GetLeadQuery(leadId));
        if (lead == null)
            throw new Exception($"Lead {leadId} not found.");

        long customerId = await _mediator.Send(new ConvertLeadToCustomerCommand(leadId));
        long agreementId = await _mediator.Send(new CreateHPAgreementCommand(customerId, lead.ProductId, lead.ProposedDownPayment, 12, 12));

        _backgroundJobClient.Enqueue<WorkflowOrchestrator>(o => o.ProcessKycAndCreditAsync(agreementId, customerId));
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ProcessKycAndCreditAsync(long agreementId, long customerId)
    {
        _logger.LogInformation("Processing KYC and Credit for Agreement: {AgreementId}", agreementId);

        var nidImage = await _mediator.Send(new GetCustomerDocumentsQuery(customerId, "NID"));
        if (nidImage == null)
        {
            _logger.LogWarning("NID document not found for Customer: {CustomerId}. Kyc pending.", customerId);
            return;
        }

        var ocrResult = await _ocrService.ExtractTextAsync(nidImage, "NID");
        _logger.LogInformation("OCR extracted NID information with confidence {Confidence}", ocrResult.Confidence);

        // Uses local LLM via service
        int creditScore = await _creditScoreService.CalculateCreditScoreAsync(customerId);
        _logger.LogInformation("Credit assessment complete. Score: {Score}", creditScore);

        await _mediator.Send(new SubmitForApprovalCommand(agreementId));
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ActivateAgreementAsync(long agreementId)
    {
        _logger.LogInformation("Activating Agreement: {AgreementId}", agreementId);

        var agreement = await _mediator.Send(new GetAgreementQuery(agreementId));
        if (agreement.Status == HPAgreementStatus.Approved)
        {
            await _mediator.Send(new ApproveHPAgreementCommand(agreementId));
            await _mediator.Send(new GenerateEmiSchedulesCommand(agreementId));

            _backgroundJobClient.Schedule<WorkflowOrchestrator>(o => 
                o.CheckDownPaymentStatusAsync(agreementId), TimeSpan.FromDays(3));
        }
    }

    public async Task CheckDownPaymentStatusAsync(long agreementId)
    {
        var agreement = await _mediator.Send(new GetAgreementQuery(agreementId));
        if (agreement.DownPayment > 0 && agreement.Payments?.Sum(p => p.Amount) < agreement.DownPayment)
        {
            _logger.LogWarning("Agreement {AgreementId} down payment is pending for 3 days.", agreementId);
            // Send notification logic here...
        }
    }

    public async Task RecordDownPaymentAndDeliverAsync(long agreementId, decimal amount, PaymentMethodEnum method)
    {
        _logger.LogInformation("Recording down payment for Agreement {AgreementId}: {Amount}", agreementId, amount);

        await _mediator.Send(new AddPaymentCommand(agreementId, amount, method));
        await _mediator.Send(new DeliverProductCommand(agreementId));
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task HandleOverdueEscalationAsync(long agreementId, int daysOverdue)
    {
        _logger.LogInformation("Handling Overdue Escalation for Agreement {AgreementId}: {DaysOverdue} days.", 
            agreementId, daysOverdue);

        if (daysOverdue >= 30 && daysOverdue < 60)
        {
            _logger.LogInformation("Assigning collector for Agreement {AgreementId}", agreementId);
            // Logic to assign a collector via command...
        }
        else if (daysOverdue >= 60 && daysOverdue < 90)
        {
            _logger.LogInformation("Generating legal notice for Agreement {AgreementId}", agreementId);
            // Logic to create LegalNotice record via command...
        }
        else if (daysOverdue >= 90)
        {
            _logger.LogWarning("Defaulting Agreement {AgreementId} due to extreme overdue.", agreementId);
            _backgroundJobClient.Enqueue<WorkflowOrchestrator>(o => o.InitiateRepossessionAsync(agreementId));
        }
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task InitiateRepossessionAsync(long agreementId)
    {
        _logger.LogInformation("Initiating Repossession for Agreement {AgreementId}", agreementId);

        await _mediator.Send(new RepossessProductCommand(agreementId));
        
        // Simulating failed recovery after repossession
        _backgroundJobClient.Schedule<WorkflowOrchestrator>(o => 
            o.WriteOffAgreementAsync(agreementId, 0), TimeSpan.FromDays(30));
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task WriteOffAgreementAsync(long agreementId, decimal writeOffAmount)
    {
        _logger.LogWarning("Writing off Agreement {AgreementId} for amount {Amount}", agreementId, writeOffAmount);

        await _mediator.Send(new WriteOffHPAgreementCommand(agreementId, writeOffAmount));
        await _mediator.Send(new CreateJournalEntryCommand(
            $"Write-off for Agreement {agreementId}", 
            writeOffAmount, "BadDebtExpense", "AccountsReceivable"));
    }
}
