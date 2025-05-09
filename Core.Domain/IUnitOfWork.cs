using Core.Domain.Interfaces;

namespace Core.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository Activities { get; }
        IRequestRepository Requests { get; }
        IUserRepository Users { get; }
        IProjectRepository Projects { get; }

        Task SaveChangeAsync(CancellationToken token = default);
    }
}
