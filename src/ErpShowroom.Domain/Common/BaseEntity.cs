using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpShowroom.Domain.Common;

public interface IDomainEvent { }
public interface IAggregateRoot { }

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }

    public Guid? RowUid { get; set; } = Guid.NewGuid();
    public int? LocationId { get; set; }
    public int? CompanyId { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? IsDeleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public long? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public string? CreatedFromIp { get; set; }
    public string? UpdatedFromIp { get; set; }
    
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    
    public string? Remarks { get; set; }
    public string? TagsJson { get; set; }          // JSON array
    public string? ExtraPropertiesJson { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
