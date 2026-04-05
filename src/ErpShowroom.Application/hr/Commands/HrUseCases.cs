using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.hr.Entities;
using ErpShowroom.Domain.prl.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.hr.Commands;

public record MarkAttendanceCommand(long EmployeeId, DateTime CheckInTime, DateTime? CheckOutTime, decimal Latitude, decimal Longitude, string SelfieImagePath) : IRequest<long>;
public class MarkAttendanceHandler(IApplicationDbContext db) : IRequestHandler<MarkAttendanceCommand, long> {
    public async Task<long> Handle(MarkAttendanceCommand request, CancellationToken ct) {
        var att = new Attendance {
            EmployeeId = request.EmployeeId,
            CheckInTime = request.CheckInTime,
            CheckOutTime = request.CheckOutTime,
            CheckInLatitude = request.Latitude,
            CheckInLongitude = request.Longitude,
            SelfieImagePath = request.SelfieImagePath,
            AttendanceDate = request.CheckInTime.Date,
            Status = AttendanceStatus.Present
        };
        db.Attendances.Add(att);
        await db.SaveChangesAsync(ct);
        return att.Id;
    }
}

public record ApplyLeaveCommand(long EmployeeId, LeaveTypeEnum LeaveType, DateTime FromDate, DateTime ToDate) : IRequest<long>;
public class ApplyLeaveHandler(IApplicationDbContext db) : IRequestHandler<ApplyLeaveCommand, long> {
    public async Task<long> Handle(ApplyLeaveCommand request, CancellationToken ct) {
        var leave = new Leave {
            EmployeeId = request.EmployeeId,
            LeaveType = request.LeaveType,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Status = LeaveStatus.Pending,
            TotalDays = (int)(request.ToDate - request.FromDate).TotalDays + 1
        };
        db.Leaves.Add(leave);
        await db.SaveChangesAsync(ct);
        return leave.Id;
    }
}

public record ProcessPayrollCommand(DateTime MonthYear) : IRequest<int>;
public class ProcessPayrollHandler(IApplicationDbContext db) : IRequestHandler<ProcessPayrollCommand, int> {
    public async Task<int> Handle(ProcessPayrollCommand request, CancellationToken ct) {
        // Implementation for processing payroll
        var processed = 0;
        var employees = await db.Employees.ToListAsync(ct);
        foreach(var emp in employees) {
            db.SalarySlips.Add(new SalarySlip { EmployeeId = emp.Id, MonthYear = request.MonthYear, GrossSalary = emp.BasicSalary, Status = SalaryStatus.Processed });
            processed++;
        }
        await db.SaveChangesAsync(ct);
        return processed;
    }
}
