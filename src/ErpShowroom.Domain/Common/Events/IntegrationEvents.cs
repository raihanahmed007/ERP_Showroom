using System;
using MassTransit;

namespace ErpShowroom.Domain.Common.Events;

public interface IIntegrationEvent : CorrelatedBy<Guid>
{
}

public abstract record IntegrationEvent
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
public class AgreementCreatedEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public long HPAgreementId { get; set; }
    public long CustomerId { get; set; }
    public string? AgreementNo { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
}

public class PaymentReceivedEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public long PaymentId { get; set; }
    public long HPAgreementId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public long? EMIId { get; set; }
}

public class SerialTransferredEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public long SerialId { get; set; }
    public long ProductId { get; set; }
    public long FromBranchId { get; set; }
    public long ToBranchId { get; set; }
    public DateTime TransferDate { get; set; }
}

public class DocumentProcessedEvent : IIntegrationEvent
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public long DocumentId { get; set; }
    public long CustomerId { get; set; }
    public string? BlobPath { get; set; }
    public string? OcrDataJson { get; set; }
    public decimal Confidence { get; set; }
}
