
using Infrastructure.Interfaces;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository Activities { get; }
        IRequestRepository Requests { get; }
        IActivityGuestsRepository ActivitiesGuests { get; }
        int Complete();
    }
}
