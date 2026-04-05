namespace ErpShowroom.Domain.Common;

public enum AccountTypeEnum { Asset, Liability, Equity, Income, Expense }
public enum VoucherTypeEnum { JV, PV, RV, CV, BPV }
public enum HPAgreementStatus { PendingApproval, Active, Closed, Defaulted, WrittenOff, Rejected }
public enum EMIPaymentStatus { Due, Partial, Paid, Overdue, Legal }
public enum PaymentMethodEnum { Cash, BankTransfer, MobileBanking, Cheque, Card }
public enum RiskBucketEnum { Current, Days1_30, Days31_60, Days61_90, Days90Plus }
public enum SerialStatusEnum { InStock, Sold, Transferred, Returned, Damaged, UnderRepair }
public enum TransferStatusEnum { Pending, InTransit, Completed, Cancelled }
public enum POStatus { Draft, Approved, PartiallyReceived, Closed, Cancelled }
public enum GRNStatus { Draft, Completed, Cancelled }
public enum LeadSourceEnum { WalkIn, Referral, Facebook, Campaign, Website, PhoneCall }
public enum LeadStatusEnum { New, Contacted, Converted, Lost }
public enum TaskStatusEnum { Pending, Completed, Cancelled }
public enum JobCardStatus { Pending, Assigned, InProgress, QualityCheck, Completed, Billed, Delivered }
public enum AttendanceStatus { Present, Absent, HalfDay, Holiday, Leave }
public enum LeaveTypeEnum { Annual, Sick, Casual, Unpaid, Maternity, Paternity }
public enum LeaveStatus { Pending, Approved, Rejected }
public enum SalaryStatus { Draft, Processed, Paid }
public enum IncentiveTypeEnum { Sales, Collection, Workshop, Referral }
public enum ApprovalDecision { Pending, Approved, Rejected, Escalated }
