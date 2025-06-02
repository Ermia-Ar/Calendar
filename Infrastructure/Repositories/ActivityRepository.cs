using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public ActivityRepository(ApplicationContext context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }

        //public async Task<List<Activity>> GettingActivitiesOwnedByTheUser(string userId, CancellationToken token
        //    , DateTime? startDate, ActivityCategory? category, bool isCompleted = false, bool isHistory = false)
        //{
        //    var activities = GetTableAsTracking()
        //        .Where(x => x.UserId == userId);

        //    if (startDate.HasValue)
        //    {
        //        activities = activities.Where(x => x.StartDate >= startDate);
        //    }
        //    if (category.HasValue)
        //    {
        //        activities = activities.Where(x => x.Category == category);
        //    }
        //    if (isHistory)
        //    {
        //        activities = activities.Where(x => x.StartDate <= DateTime.Now);
        //    }
        //    else
        //    {
        //        activities = activities.Where(x => x.StartDate >= DateTime.Now);
        //    }

        //    return await activities.ToListAsync(token);
        //}

        //public async Task<List<Activity>> GetHistoryOfUserActivities(string userId, CancellationToken token)
        //{
            //var activities = await GetTableAsTracking()
            //    .Where(x => x.StartDate < DateTime.Now && x.UserId == userId)
            //    .ToListAsync(token);

            //return activities;
        //}

        public void UpdateActivity(Activity UpdateActivity)
        {
            _context.Activities.Update(UpdateActivity);
        }

        public async Task<List<Activity>> GetProjectActivities(string projectId, CancellationToken token
            , DateTime? startDate = null)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("projectId", projectId);
            parameters.Add("startDate", startDate);


            var activities = await connection.QueryAsync<Activity>(
                "Sp_GetProjectActivities",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
                );

            return activities.ToList();
        }

        public async Task<string[]> GetProjectActiveActivityIds(string projectId, CancellationToken token)
        {
            var activities = (await GetProjectActivities(projectId, token))
                .Select(x => x.Id).ToArray();
            
            return activities;
        }

        public async Task AddActivity(Activity activity, CancellationToken token)
        {
            await _context.Activities.AddAsync(activity, token);
        }

        public void DeleteActivity(Activity activity)
        {
            _context.Activities.Remove(activity);
        }

        public void DeleteRangeActivities(ICollection<Activity> activities)
        {
            _context.Activities.RemoveRange(activities);
        }

        public async Task<Activity?> GetActivityById(string id, CancellationToken token)
        {
            return await _context.Activities.FindAsync(id, token);
        }
    }
}
