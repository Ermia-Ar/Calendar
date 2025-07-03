namespace Infrastructure.Persistance.Repositories;

public class RequestRepository : IRequestsRepository
{
	private readonly ApplicationContext _context;
	private readonly IConfiguration _configuration;

	public RequestRepository(ApplicationContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}
	//Commands
	public void Remove(Request request)
	{
		_context.Requests.Remove(request);
	}

	public void RemoveRange(ICollection<Request> requests)
	{
		_context.Requests.RemoveRange(requests);
	}

	public void Add(Request request)
	{
		_context.Requests.Add(request);
	}

	public void AddRange(ICollection<Request> requests)
	{
		_context.Requests.AddRange(requests);
	}

	public void Update(Request request)
	{
		_context.Requests.Update(request);
	}


	//Queries
	public async Task<IReadOnlyCollection<Request>> Find(string? projectId
		, string? activityId, string? receiverId, string? senderId
		, RequestStatus? status, CancellationToken token, bool? isGuest = null)
	{
		return await _context.Requests.Where
			(x => (projectId != null ? x.ProjectId == projectId : true)
			&& (activityId != null ? x.ActivityId == activityId : true)
			&& (receiverId != null ? x.ReceiverId == receiverId : true)
			&& (senderId != null ? x.SenderId == senderId : true)
			&& (isGuest.HasValue ? x.IsGuest == isGuest : true)
			&& (status.HasValue ? x.Status == status : true)
			)
			.ToListAsync();
	}

	public async Task<ListDto> GetAll(GetAllRequestFiltering filtering,
		GetAllRequestsOrdring ordering, PaginationFilter pagination,
		CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("IsArchived", true);

		// TODO : for senderId , receiverId filtering
		//filtring
		parameters.Add("SenderId", filtering.SenderId);
		parameters.Add("ReceiverId", filtering.ReceiverId);
		parameters.Add("RequestFor", filtering.RequestFor);
		parameters.Add("Status", filtering.Status);
		parameters.Add("ProjectId", filtering.ProjectId);
		parameters.Add("ActivityId", filtering.ActivityId);
		//ordring
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var userRequests = await connection.QueryAsync<GetAllRequestQueryResponse>
		("SP_GetRequests", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, userRequests.ToImmutableList());

	}

	public async Task<IResponse?> GetById(string id, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("requestId", id);

		var userRequests = await connection.QueryAsync<GetRequestByIdQueryResponse>
			("SP_GetRequests", parameters, commandType: CommandType.StoredProcedure);

		return userRequests.FirstOrDefault();
	}

	public async Task<ListDto> GetAllProjects(string userId, GetAllProjectsFiltering filtering
		, GetAllProjectsOrdring ordering, PaginationFilter pagination
		, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", (int)RequestFor.Project);
		parameters.Add("status", RequestStatus.Accepted);
		parameters.Add("receiverId", userId);
		//filtering
		parameters.Add("startDate", filtering.StartDate);
		parameters.Add("ownerId", filtering.UserIsOwner ? userId : null);
		parameters.Add("History", filtering.IsHistory == true ? DateTime.Now : null);
		//ordring
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var projects = await connection.QueryAsync<GetAllProjectQueryResponse>
			("SP_GetProjects", parameters, commandType: CommandType.StoredProcedure);

		var TotalCount = parameters.Get<int>("TotalCount");


		return new ListDto(TotalCount, projects.ToImmutableList());
	}

	public async Task<IReadOnlyCollection<IResponse>> GetMemberOfProject(string projectId, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", RequestFor.Project);
		parameters.Add("projectId", projectId);
		parameters.Add("status", RequestStatus.Accepted);

		var members = await connection.QueryAsync<GetMemberOfProjectQueryResponse>
			("SP_GetMemberOfProject", parameters, commandType: CommandType.StoredProcedure);


		return members.ToImmutableList();
	}

	public async Task<ListDto> GetAllActivities(string userId, string projectId, GetAllActivitiesFiltering filtering
		, GetAllActivitiesOrdering ordering, PaginationFilter pagination
		, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", (int)RequestFor.Activity);
		parameters.Add("receiverId", userId);
		parameters.Add("status", (int)RequestStatus.Accepted);
		parameters.Add("projectId ", projectId);
		//filtering
		parameters.Add("isCompleted", filtering.IsCompleted);
		parameters.Add("category", filtering.Category);
		parameters.Add("History", filtering.IsHistory == true ? DateTime.Now : null);
		parameters.Add("startDate", filtering.StartDate);
		//ordring
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


		var activities = await connection.QueryAsync<GetAllActivitiesQueryResponse>
			("SP_GetActivities", parameters, commandType: CommandType.StoredProcedure);

		int totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, activities.ToImmutableList());
	}

	public async Task<IReadOnlyCollection<IResponse>> GetMemberOfActivity(string activityId, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", RequestFor.Activity);
		parameters.Add("activityId", activityId);
		parameters.Add("status", (int)RequestStatus.Accepted);

		var members = await connection.QueryAsync<GetMemberOfActivityQueryResponse>
			("SP_GetMemberOfActivity", parameters, commandType: CommandType.StoredProcedure);


		return members.ToImmutableList();
	}

	public async Task<string[]> FindMemberIdsOfProject(string projectId, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", RequestFor.Project);
		parameters.Add("projectId", projectId);
		parameters.Add("status", RequestStatus.Accepted);

		var members = await connection.QueryAsync<string>
			("SP_GetMemberIdsOfProject", parameters, commandType: CommandType.StoredProcedure);


		return members.ToArray();
	}

	public async Task<string[]> FindMemberIdsOfActivity(string activityId, CancellationToken token)
	{
		using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("isExpire", true);
		parameters.Add("requestFor", RequestFor.Activity);
		parameters.Add("activityId", activityId);
		parameters.Add("status", (int)RequestStatus.Accepted);

		var members = await connection.QueryAsync<string>
			("SP_GetMemberIdsOfActivity", parameters, commandType: CommandType.StoredProcedure);


		return members.ToArray();
	}

	public async Task<Request?> FindById(string id, CancellationToken token)
	{
		return await _context.Requests
			.FirstOrDefaultAsync(x => x.Id == id, token);
	}

	public async Task<List<Request>> FindMember(string? projectId
		, string? activityId, string receiverId, CancellationToken token)
	{
		return await _context.Requests.Where
			(x => (projectId != null ? x.ProjectId == projectId : true)
			&& (activityId != null ? x.ActivityId == activityId : true)
			&& x.ReceiverId == receiverId
			&& x.Status == RequestStatus.Accepted)
			.ToListAsync();
	}

	public async Task<string[]> FindProjectIds(string userId, CancellationToken token)
	{
		var projectIds = await _context.Requests.Where(x => x.RequestFor == RequestFor.Project
			&& x.Status == RequestStatus.Accepted
			&& x.ReceiverId == userId)
			.Select(x => x.ProjectId)
			.ToArrayAsync();

		return projectIds;
	}

	public async Task<string[]> FindActivityIds(string userId, CancellationToken token)
	{
		var activityIds = await _context.Requests.Where(x => x.RequestFor == RequestFor.Activity
			&& x.Status == RequestStatus.Accepted
			&& x.ReceiverId == userId)
			.Select(x => x.ActivityId)
			.ToArrayAsync();

		return activityIds;
	}

	public async Task<Request?> FindOne(string activityId, string receiverId, CancellationToken token)
	{
		var request = (await FindMember(null, activityId, receiverId, token))
								.FirstOrDefault();
		return request;
	}


}
