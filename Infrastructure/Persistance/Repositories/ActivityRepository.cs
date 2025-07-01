using SharedKernel.Helper;

namespace Infrastructure.Persistance.Repositories;

public class ActivityRepository(ApplicationContext context,
                                IConfiguration configuration) : IActivitiesRepository
{
    private readonly ApplicationContext _context = context;
    private readonly IConfiguration _configuration = configuration;


    //Commands
    public void Update(Activity UpdateActivity)
    {
        _context.Activities.Update(UpdateActivity);
    }
    public void Add(Activity activity)
    {
        _context.Activities.Add(activity);
    }

    public void Delete(Activity activity)
    {
        _context.Activities.Remove(activity);
    }

    public void RemoveRange(ICollection<Activity> activities)
    {
        _context.Activities.RemoveRange(activities);
    }

    //Queries

    public async Task<string[]> GetActiveActivitiesId(string projectId, CancellationToken token)
    {

        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("projectId", projectId);
        parameters.Add("startDate", DateTime.Now);


        var activities = await connection.QueryAsync<string>(
            "Sp_GetProjectActiveActivityIds",
            parameters,
            commandType: CommandType.StoredProcedure
            );

        return activities.ToArray();
    }

    public async Task<IResponse?> GetById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("activityId", id);


        var activities = await connection.QueryAsync<GetActivityByIdQueryResponse>(
            "SP_GetActivityById",
            parameters,
            commandType: CommandType.StoredProcedure
            );

        return activities.FirstOrDefault();
    }

    public async Task<Activity?> FindById(string id, CancellationToken token)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<List<Activity>> Find(string? projectId, CancellationToken token)
    {
        return await _context.Activities.Where(x => x.ProjectId == projectId)
            .ToListAsync(token);
    }
}
