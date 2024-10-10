using Artemis.Data.Repositories.Interfaces;

namespace Artemis.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICatRepository Cats { get; }

        ITagRepository Tags { get; }

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        Task SaveChangesAsync();
    }
}