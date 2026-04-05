using System;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.fin.Events;

public record AgreementApprovedEvent(long AgreementId, long CustomerId, DateTime OccurredOn) : IDomainEvent;
public record EMIComputedEvent(long AgreementId, int TotalInstallments, DateTime OccurredOn) : IDomainEvent;
public record AgreementDefaultedEvent(long AgreementId, DateTime OccurredOn) : IDomainEvent;
public record AgreementWrittenOffEvent(long AgreementId, decimal WrittenOffAmount, DateTime OccurredOn) : IDomainEvent;
public record CollectorAssignedEvent(long AgreementId, long CollectorId, DateTime OccurredOn) : IDomainEvent;
public record PaymentReceivedEvent(long PaymentId, long AgreementId, decimal Amount, DateTime OccurredOn) : IDomainEvent;
public record PenaltyAppliedEvent(long PenaltyId, long EMIId, decimal Amount, DateTime OccurredOn) : IDomainEvent;
public record RiskBucketUpdatedEvent(long AgreementId, string OldBucket, string NewBucket, DateTime OccurredOn) : IDomainEvent;
public record LegalEscalatedEvent(long AgreementId, long LegalCaseId, DateTime OccurredOn) : IDomainEvent;
