using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Infrastructure.Base;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Entity;
using Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ActivityGuestsRepository : GenericRepositoryAsync<ActivityGuest>, IActivityGuestsRepository
    {
        private DbSet<ActivityGuest> _activityGuests;

        public ActivityGuestsRepository(ApplicationContext context) : base(context) 
        {
            _activityGuests = context.Set<ActivityGuest>();
        }

        public async Task DeleteByActivityId(string activityId)
        {
            var items = await GetTableNoTracking()
                .Where(x => x.ActivityId == activityId)
                .ToListAsync();

            await DeleteRangeAsync(items);
        }

        public async Task<List<UserResponse>> GetActivityGuestByActivityId(string activityId)
        {
            var ActivityIdParam = new SqlParameter("@ActivityId", activityId);

            var guests = await _dbContext.Set<UserResponse>()
                .FromSqlRaw("EXEC Sp_Get_Guest @ActivityId" , ActivityIdParam)
                .AsNoTracking()
                .ToListAsync();

            return guests;
        }

        public async Task<List<ActivityResponse>> GetActivityGuestByUserId(string userId)
        {
            var UserIdParam = new SqlParameter("@UserId", userId);

            var activities = await _dbContext.Set<ActivityResponse>()
                .FromSqlRaw("EXEC Sp_Get_ActivityGuest @UserId", UserIdParam)
                .AsNoTracking()
                .ToListAsync();

            return activities;
        }

        public async Task<bool> RemoveGuestById(string userId, string activityId)
        {
            try
            {
                var guest = await GetTableNoTracking()
                    .FirstAsync(x => x.UserId == userId && x.ActivityId == activityId);

                await DeleteAsync(guest);
                return true;
            }
            catch
            {
                return false;
            }
        }

       
    }
}
