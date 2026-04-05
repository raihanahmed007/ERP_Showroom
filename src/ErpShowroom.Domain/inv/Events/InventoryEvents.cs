using System;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.inv.Events;

public record StockReservedEvent(long ProductId, int Quantity, DateTime OccurredOn) : IDomainEvent;
public record StockReleasedEvent(long ProductId, int Quantity, DateTime OccurredOn) : IDomainEvent;
public record StockShippedEvent(long ProductId, int Quantity, DateTime OccurredOn) : IDomainEvent;
public record SerialStatusChangedEvent(long SerialId, string OldStatus, string NewStatus, DateTime OccurredOn) : IDomainEvent;
