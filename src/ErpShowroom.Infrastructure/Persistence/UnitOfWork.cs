using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    public IRepository<HPAgreement> HPAgreements => Repository<HPAgreement>();
    public IRepository<EMISchedule> EMISchedules => Repository<EMISchedule>();

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repository = new Repository<T>(_context);
            _repositories[type] = repository;
        }

        return (IRepository<T>)_repositories[type];
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
