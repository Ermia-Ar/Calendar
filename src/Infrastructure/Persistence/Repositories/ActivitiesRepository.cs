using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories;

public class ActivitiesRepository(ApplicationContext context, IConfiguration configuration)
	: IActivitiesRepository
{
    private readonly ApplicationContext _context = context;
	private readonly string _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];



	//Commands
	public void Update(Activity activity)
    {
        _context.Activities.Update(activity);
    }

    public Activity Add(Activity activity)
    {
        return  _context.Activities.Add(activity).Entity;
    }

    public List<Activity> AddRange(ICollection<Activity> activities)
    {
        List<Activity> list = new List<Activity>();
        foreach (Activity activity in activities)
        {
            list.Add(_context.Add(activity).Entity);
        }
        return list;
    }

    //Queries

    public async Task<long[]> GetActiveActivitiesIds(long projectId, CancellationToken token)
    {
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("projectId", projectId);
		parameters.Add("startDate", DateTime.UtcNow);
		parameters.Add("isCompleted", true);

		var ids = await connection.QueryAsync<long>
			("Sp_GetProjectActiveActivityIds", parameters, commandType: CommandType.StoredProcedure);

		return ids.ToArray();
	}

    public async Task<IResponse?> GetById(long id, CancellationToken token)
    {
	    await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("activityId", id);

		var activity = await connection.QueryAsync<GetActivityByIdQueryResponse>
			("SP_GetActivityById", parameters, commandType: CommandType.StoredProcedure);

        return activity.FirstOrDefault();
	}

    public async Task<Activity?> FindById(long id, CancellationToken token)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(x => x.Id == id, token);

    }

	public async Task RemoveById(long id, CancellationToken token)
	{
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("activityId", id);

        await connection.QueryAsync("SP_SoftDeleteActivityAndCommentsAndActivityMembersAndNotificaitons",
            parameters, commandType: CommandType.StoredProcedure);
	}
}
