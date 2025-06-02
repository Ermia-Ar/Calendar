using Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository Activities { get; }
        IRequestRepository Requests { get; }
        ICommentRepository Comments { get; }
        IProjectRepository Projects { get; }
        IUserRepository Users { get; }

        Task SaveChangeAsync(CancellationToken token = default);
        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
