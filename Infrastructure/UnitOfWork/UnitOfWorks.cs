using Core.Domain;
using Core.Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IActivityRepository Activities { get; private set; }

        public IRequestRepository Requests { get; private set; }

        public IProjectRepository Projects { get; private set; }

        public IUserRepository Users { get; private set; }

        public UnitOfWorks(ApplicationContext context,
            IActivityRepository activities, IRequestRepository requests,
            IProjectRepository projects, IUserRepository users)
        {
            _context = context;
            Activities = activities;
            Requests = requests;
            Projects = projects;
            Users = users;
        }

        // did not use cause there is async methods 
        

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangeAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }
    }
}
