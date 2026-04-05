using System;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.acc.Events;

public record JournalBalancedEvent(long JournalId, decimal TotalAmount, DateTime OccurredOn) : IDomainEvent;
