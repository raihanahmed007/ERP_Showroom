using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.fin.Commands;

public record CreateHPAgreementCommand(long CustomerId, long ProductId, decimal DownPayment, int InstallmentCount, decimal InterestRate) : IRequest<long>;
public class CreateHPAgreementHandler(IUnitOfWork uow) : IRequestHandler<CreateHPAgreementCommand, long>
{
    public async Task<long> Handle(CreateHPAgreementCommand request, CancellationToken ct)
    {
        var agreement = new HPAgreement
        {
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            DownPayment = request.DownPayment,
            InstallmentCount = request.InstallmentCount,
            InterestRate = request.InterestRate,
            Status = HPAgreementStatus.PendingApproval,
            AgreementDate = DateTime.UtcNow
        };
        await uow.HPAgreements.AddAsync(agreement, ct);
        await uow.SaveChangesAsync(ct);
        return agreement.Id;
    }
}

public record AddPaymentCommand(long HPAgreementId, long EMIId, decimal Amount, PaymentMethodEnum PaymentMethod, string ReferenceNo, long CollectorEmployeeId) : IRequest<long>;
public class AddPaymentHandler(IApplicationDbContext db) : IRequestHandler<AddPaymentCommand, long>
{
    public async Task<long> Handle(AddPaymentCommand request, CancellationToken ct)
    {
        var payment = new Payment
        {
            HPAgreementId = request.HPAgreementId,
            EMIId = request.EMIId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            ReferenceNo = request.ReferenceNo,
            CollectorEmployeeId = request.CollectorEmployeeId,
            PaymentDate = DateTime.UtcNow
        };
        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);
        return payment.Id;
    }
}

public record ApplyPenaltyCommand(long EMIId) : IRequest<long>;
public class ApplyPenaltyHandler(IApplicationDbContext db) : IRequestHandler<ApplyPenaltyCommand, long>
{
    public async Task<long> Handle(ApplyPenaltyCommand request, CancellationToken ct)
    {
        var emi = await db.EMISchedules.FirstOrDefaultAsync(e => e.Id == request.EMIId, ct);
        if (emi == null) throw new Exception("EMI not found");

        var penalty = new Penalty
        {
            EMIId = request.EMIId,
            PenaltyDate = DateTime.UtcNow,
            PenaltyAmount = 500
        };
        db.Penalties.Add(penalty);
        await db.SaveChangesAsync(ct);
        return penalty.Id;
    }
}

public record AssignCollectorCommand(long HPAgreementId, long CollectorEmployeeId) : IRequest<bool>;
public class AssignCollectorHandler(IApplicationDbContext db) : IRequestHandler<AssignCollectorCommand, bool>
{
    public async Task<bool> Handle(AssignCollectorCommand request, CancellationToken ct)
    {
        var board = await db.RecoveryBoards.FirstOrDefaultAsync(r => r.HPAgreementId == request.HPAgreementId, ct);
        if (board != null)
        {
            board.AssignedCollectorId = request.CollectorEmployeeId;
            await db.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
}

public record WriteOffAgreementCommand(long HPAgreementId, decimal Amount) : IRequest<bool>;
public class WriteOffAgreementHandler(IApplicationDbContext db) : IRequestHandler<WriteOffAgreementCommand, bool>
{
    public async Task<bool> Handle(WriteOffAgreementCommand request, CancellationToken ct)
    {
        var agreement = await db.HPAgreements.FirstOrDefaultAsync(a => a.Id == request.HPAgreementId, ct);
        if (agreement == null) return false;
        
        agreement.Status = HPAgreementStatus.WrittenOff;
        agreement.WrittenOffAmount = request.Amount;
        agreement.WrittenOffAt = DateTime.UtcNow;
        
        await db.SaveChangesAsync(ct);
        return true;
    }
}

public record GetOverdueAgreementsQuery(RiskBucketEnum RiskBucket) : IRequest<List<RecoveryBoard>>;
public class GetOverdueAgreementsHandler(IApplicationDbContext db) : IRequestHandler<GetOverdueAgreementsQuery, List<RecoveryBoard>>
{
    public Task<List<RecoveryBoard>> Handle(GetOverdueAgreementsQuery request, CancellationToken ct)
    {
        return db.RecoveryBoards
            .Where(r => r.RiskBucket == request.RiskBucket)
            .ToListAsync(ct);
    }
}
