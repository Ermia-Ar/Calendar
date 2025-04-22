using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IActivityRepository Activities { get; private set; }

        public IRequestRepository Requests { get; private set; }

        public IActivityGuestsRepository ActivitiesGuests { get; private set; }

        public UnitOfWorks(ApplicationContext context)
        {
            _context = context;
            Activities = new ActivityRepository(_context);
            Requests = new RequestRepository(_context);
            ActivitiesGuests = new ActivityGuestsRepository(_context);
        }

        // did not use cause there is async methods 
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
