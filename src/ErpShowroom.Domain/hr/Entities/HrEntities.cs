using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.prl.Entities;

namespace ErpShowroom.Domain.hr.Entities;

public class Employee : BaseEntity
{
    [Required, MaxLength(50)] public string? EmployeeCode { get; set; }
    [Required, MaxLength(200)] public string? FullName { get; set; }
    public string? Designation { get; set; }
    public string? Department { get; set; }
    public DateTime? JoiningDate { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public decimal? BasicSalary { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? NIDNumber { get; set; }
    public string? BloodGroup { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BankAccountNo { get; set; }
    public string? PFNumber { get; set; }
    public long? ReportingToEmployeeId { get; set; }

    public virtual ICollection<Attendance>? Attendances { get; set; }
    public virtual ICollection<Leave>? Leaves { get; set; }
    public virtual ICollection<SalarySlip>? SalarySlips { get; set; }
    public virtual ICollection<Incentive>? Incentives { get; set; }
}

public class Attendance : BaseEntity
{
    public long? EmployeeId { get; set; }
    public DateTime? AttendanceDate { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public decimal? CheckInLatitude { get; set; }
    public decimal? CheckInLongitude { get; set; }
    public bool? IsLate { get; set; } = false;
    public int? LateMinutes { get; set; }
    public AttendanceStatus? Status { get; set; } = AttendanceStatus.Absent;
    public string? DeviceInfo { get; set; }
    public string? SelfieImagePath { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}

public class Leave : BaseEntity
{
    public long? EmployeeId { get; set; }
    public LeaveTypeEnum? LeaveType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Reason { get; set; }
    public long? ApprovedByUserId { get; set; }
    public LeaveStatus? Status { get; set; } = LeaveStatus.Pending;
    public DateTime? ApprovedAt { get; set; }
    public int? TotalDays { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}

public class LeaveBalance : BaseEntity
{
    public long? EmployeeId { get; set; }
    public int? Year { get; set; }
    public LeaveTypeEnum? LeaveType { get; set; }
    public decimal? TotalDays { get; set; }
    public decimal? UsedDays { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }
}
