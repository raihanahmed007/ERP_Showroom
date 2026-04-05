using System;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Application.Common.Models;

public class RecoveryDashboardItem
{
    public string AgreementNo { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalOverdue { get; set; }
    public RiskBucketEnum? RiskBucket { get; set; }
}

public class ParReportResult
{
    public int BranchId { get; set; }
    public decimal Portfolio { get; set; }
    public decimal Overdue30Plus { get; set; }
    public decimal PAR30 => Portfolio > 0 ? (Overdue30Plus / Portfolio) * 100 : 0;
}
