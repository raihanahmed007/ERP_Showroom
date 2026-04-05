using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Domain.wf.Entities;

public class WorkflowDefinition : BaseEntity
{
    [Required, MaxLength(100)] public string? WorkflowName { get; set; }
    public string? EntityName { get; set; }
    public bool? IsActive { get; set; } = true;
    public int? TimeoutHours { get; set; }

    public virtual ICollection<WorkflowStep>? Steps { get; set; }
}

public class WorkflowStep : BaseEntity
{
    public long? WorkflowId { get; set; }
    public int? StepOrder { get; set; }
    public string? RequiredRole { get; set; }
    public int? ApprovalTimeoutHours { get; set; }
    public string? EscalationRole { get; set; }
    public bool? IsParallel { get; set; } = false;
    public string? ConditionExpression { get; set; }

    [ForeignKey(nameof(WorkflowId))]
    public virtual WorkflowDefinition? Workflow { get; set; }
}

public class ApprovalHistory : BaseEntity
{
    public long? WorkflowId { get; set; }
    public long? EntityId { get; set; }
    public long? StepId { get; set; }
    public long? ApproverUserId { get; set; }
    public ApprovalDecision? Decision { get; set; } = ApprovalDecision.Pending;
    public string? Comments { get; set; }
    public DateTime? ActionAt { get; set; } = DateTime.UtcNow;
    public string? RequestDataSnapshot { get; set; }

    [ForeignKey(nameof(WorkflowId))]
    public virtual WorkflowDefinition? Workflow { get; set; }
}
