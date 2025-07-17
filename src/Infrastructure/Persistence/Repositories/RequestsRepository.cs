namespace Infrastructure.Persistence.Repositories;

public class RequestsRepository(
	ApplicationContext context,
	IConfiguration configuration)
	: IRequestsRepository
{

	private readonly ApplicationContext _context = context;
	private readonly string? _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];

	//Commands
	public void Remove(ActivityRequest activityRequest)
	{
		_context.Requests.Remove(activityRequest);
	}

	public void RemoveRange(ICollection<ActivityRequest> requests)
	{
		_context.Requests.RemoveRange(requests);
	}

	public void Add(ActivityRequest activityRequest)
	{
		_context.Requests.Add(activityRequest);
	}

	public void AddRange(ICollection<ActivityRequest> requests)
	{
		_context.Requests.AddRange(requests);
	}

	public void Update(ActivityRequest activityRequest)
	{
		_context.Requests.Update(activityRequest);
	}


	//Queries

	public async Task<ListDto> GetAll(GetAllRequestFiltering filtering,
		GetAllRequestsOrdring ordering, PaginationFilter pagination,
		CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();

		// TODO : for senderId , receiverId filtering
		//filtering
		parameters.Add("ReceiverId", filtering.ReceiverId);
		parameters.Add("ActivityId", filtering.ActivityId);
		parameters.Add("InvitedAt", filtering.InviteAt);
		//ordering
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

	public async Task<IResponse?> GetById(long id, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("requestId", id);

		var userRequests = await connection.QueryAsync<GetRequestByIdQueryResponse>
			("SP_GetRequestById", parameters, commandType: CommandType.StoredProcedure);

		return userRequests.FirstOrDefault();
	}

	public async Task<ActivityRequest?> FindById(long id, CancellationToken token)
	{
		return await _context.Requests
			.FirstOrDefaultAsync(x => x.Id == id, token);
	}
	
}
