using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.Application.wrk.Commands;

public record CreateJobCardCommand(long CustomerId, string VehicleVIN, string CustomerComplaint, long AssignedTechnicianId) : IRequest<long>;
public class CreateJobCardHandler(IApplicationDbContext db) : IRequestHandler<CreateJobCardCommand, long> {
    public async Task<long> Handle(CreateJobCardCommand request, CancellationToken ct) {
        var job = new JobCard {
            CustomerId = request.CustomerId,
            VehicleVIN = request.VehicleVIN,
            CustomerComplaint = request.CustomerComplaint,
            AssignedTechnicianId = request.AssignedTechnicianId,
            JobCardNo = "JC-" + Guid.NewGuid(),
            ReceivedDate = DateTime.UtcNow,
            Status = JobCardStatus.Pending
        };
        db.JobCards.Add(job);
        await db.SaveChangesAsync(ct);
        return job.Id;
    }
}

public record CompleteJobCardCommand(long JobCardId, decimal TotalCost) : IRequest<bool>;
public class CompleteJobCardHandler(IApplicationDbContext db) : IRequestHandler<CompleteJobCardCommand, bool> {
    public async Task<bool> Handle(CompleteJobCardCommand request, CancellationToken ct) {
        var job = await db.JobCards.FirstOrDefaultAsync(j => j.Id == request.JobCardId, ct);
        if (job != null) {
            job.TotalCost = request.TotalCost;
            job.CompletedDate = DateTime.UtcNow;
            job.Status = JobCardStatus.Completed;
            await db.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
}
