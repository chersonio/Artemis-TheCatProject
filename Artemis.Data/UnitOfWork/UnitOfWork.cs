using Artemis.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _context;
    private IDbContextTransaction _transaction;

    public ICatRepository Cats { get; }
    public ITagRepository Tags { get; }

    public UnitOfWork(ApplicationDBContext context, ICatRepository cats, ITagRepository tags)
    {
        _context = context;
        Cats = cats;
        Tags = tags;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction?.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction?.RollbackAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
