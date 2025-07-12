using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Application.ApplicationServices.Activities.Queries.GetMembers;
using Core.Application.ApplicationServices.Projects.Queries.Activities;
using Core.Domain.Entities.ActivityMembers;

namespace Infrastructure.Persistence.Repositories;

public class ActivityMembersRepository(
	ApplicationContext context,
	IConfiguration configuration) 
	: IActivityMembersRepository
{
	private readonly ApplicationContext _context = context;
	private readonly string _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];


	//Commands
	public ActivityMember Add(ActivityMember activityMember)
	{
		return _context.ActivityMembers.Add(activityMember).Entity;
	}

	public void AddRange(ICollection<ActivityMember> activityMembers)
	{
		_context.ActivityMembers.AddRange(activityMembers);
	}
	
	public void Remove(ActivityMember activityMember)
	{
		_context.ActivityMembers.Remove(activityMember);
	}

	public void RemoveRange(ICollection<ActivityMember> activityMembers)
	{
		_context.ActivityMembers.RemoveRange(activityMembers);
	}
	
	//Queries
	public async Task<ActivityMember?> FindById(long id, CancellationToken token)
	{
		 return	await _context.ActivityMembers
			 .FirstOrDefaultAsync(f => f.Id == id, token);
	}

	public async Task<ListDto> GetActivitiesForUserId(Guid userId, long projectId,
		GetAllActivitiesFiltering filtering, GetAllActivitiesOrdering ordering, 
		PaginationFilter pagination, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("memberId", userId);
		// default projectId
		parameters.Add("projectId", projectId);
		parameters.Add("isGuest", true);
		parameters.Add("isActive", true);
		//filtering
		parameters.Add("startDate", filtering.StartDate);
		parameters.Add("category", filtering.Category);
		parameters.Add("isCompleted", filtering.IsCompleted);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var activities = await connection.QueryAsync<GetAllActivitiesQueryResponse>
			("SP_GetActivities", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, activities.ToImmutableList());
	}

	public async Task<ListDto> GetActivitiesOfProjectForUserId(Guid userId, long projectId, 
		GetProjectActivitiesFiltering filtering, GetProjectActivitiesOrdering ordering, 
		PaginationFilter pagination, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("memberId", userId);
		// default projectId
		parameters.Add("projectId", projectId);
		parameters.Add("isGuest", true);
		parameters.Add("isActive", true);
		//filtering
		parameters.Add("startDate", filtering.StartDate);
		parameters.Add("category", filtering.Category);
		parameters.Add("isCompleted", filtering.IsCompleted);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var activities = await connection.QueryAsync<GetProjectActivitiesQueryResponse>
			("SP_GetActivities", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, activities.ToImmutableList());

	}

	public async Task<IReadOnlyCollection<long>> FindActivityMemberIdsOfActivity(long activityId, CancellationToken token)
	{
		//FindActivityMemberIdsOfActivity
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("activityId", activityId, DbType.Int64);

		var ids = await connection.QueryAsync<long>
			("SP_FindActivityMemberIdsOfActivity", parameters, commandType: CommandType.StoredProcedure);

		return ids.ToImmutableList();
	}

	public async Task<ActivityMember?> FindByActivityIdAndMemberId(Guid userId, long activityId, CancellationToken token)
	{
		return await _context.ActivityMembers
			.FirstOrDefaultAsync(x => x.MemberId == userId && 
				x.ActivityId == activityId, token); 
	}

	public async Task<IReadOnlyCollection<Guid>> FindMemberIdsOfActivity(long activityId, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("activityId", activityId, DbType.Int64);

		var memberIds = await connection.QueryAsync<Guid>
			("SP_FindMemberIdsOfActivity", parameters, commandType: CommandType.StoredProcedure);

		return memberIds.ToImmutableList();

	}

	public async Task<bool> IsMemberOfActivity(Guid userId, long activityId, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("activityId", activityId, DbType.Int64);
		parameters.Add("memberId", userId, DbType.Guid);
		parameters.Add("isMember", dbType: DbType.Boolean, direction: ParameterDirection.Output);

		await connection.QueryAsync
			("SP_IsMemberOfActivity", parameters, commandType: CommandType.StoredProcedure);

		var isMember = parameters.Get<bool>("isMember");


		return isMember;
	}

	public async Task<IReadOnlyCollection<ActivityMember>> FindByActivityId(long activityId, CancellationToken token)
	{
		var activities = await _context.ActivityMembers
			.Where(x => x.ActivityId == activityId)
			.ToListAsync(token);

		return activities.ToImmutableList();
	}

	public async Task<IReadOnlyCollection<ActivityMember>> FindActivityMemberOfActivitiesForProjectForUserId(Guid userId,
		long projectId, CancellationToken token)
	{
		var activities = await _context.ActivityMembers
			.Include(x => x.Activity)
			.Where(x => x.Activity.ProjectId == projectId && x.MemberId == userId)
			.ToListAsync(token);

		return activities.ToImmutableList();
	}

	public async Task<ListDto> GetMemberOfActivity(long activityId,
		PaginationFilter pagination, 
		CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();

		parameters.Add("activityId", activityId);

		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var members = await connection.QueryAsync<GetMemberOfActivityQueryResponse>
			("SP_GetMembersOfActivity", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, members.ToImmutableList());
	}


}
