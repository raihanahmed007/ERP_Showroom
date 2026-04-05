using System;
using MediatR;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.fin.WorkflowCommands;

public record ConvertLeadToCustomerCommand(long LeadId) : IRequest<long>;
public record CreateHPAgreementCommand(long CustomerId, long ProductId, decimal DownPayment, int InstallmentCount, decimal InterestRate) : IRequest<long>;
public record SubmitForApprovalCommand(long AgreementId) : IRequest<bool>;
public record ApproveHPAgreementCommand(long AgreementId) : IRequest<bool>;
public record GenerateEmiSchedulesCommand(long AgreementId) : IRequest<bool>;
public record AddPaymentCommand(long AgreementId, decimal Amount, PaymentMethodEnum Method, long? EmiId = null) : IRequest<long>;
public record DeliverProductCommand(long AgreementId) : IRequest<bool>;
public record RepossessProductCommand(long AgreementId) : IRequest<bool>;
public record WriteOffHPAgreementCommand(long AgreementId, decimal Amount) : IRequest<bool>;
public record CreateJournalEntryCommand(string Description, decimal Amount, string DebitAccount, string CreditAccount) : IRequest<long>;

// Query skeletons
public record GetLeadQuery(long LeadId) : IRequest<LeadDto>;
public record GetCustomerDocumentsQuery(long CustomerId, string DocumentType) : IRequest<byte[]?>;
public record GetAgreementQuery(long AgreementId) : IRequest<HPAgreement>;

public class LeadDto 
{ 
    public long Id { get; set; }
    public long ProductId { get; set; }
    public decimal ProposedDownPayment { get; set; }
}
