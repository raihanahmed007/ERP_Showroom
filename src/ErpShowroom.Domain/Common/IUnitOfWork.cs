using System;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Domain.Common;

public interface IUnitOfWork : IDisposable
{
    // Example explicitly requested properties
    IRepository<HPAgreement> HPAgreements { get; }
    IRepository<EMISchedule> EMISchedules { get; }
    
    // Generic fallback for all other repositories
    IRepository<T> Repository<T>() where T : BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
