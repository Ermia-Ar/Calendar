namespace Infrastructure.Persistence.Repositories;

public class CommentsRepository(ApplicationContext context
							  , IConfiguration configuration) : ICommentsRepository
{
	private readonly ApplicationContext _context = context;
	private readonly string _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];


	//Commands
	public void Add(Comment comment)
	{
		_context.Comments.AddAsync(comment);
	}

	public void Remove(Comment comment)
	{
		_context.Comments.Remove(comment);
	}

	public void RemoveRange(ICollection<Comment> comments)
	{
		_context.Comments.RemoveRange(comments);
	}


	//Queries
	public async Task<IResponse?> GetById(long id, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("commentId", id);

		var activity = await connection.QueryAsync<GetCommentByIdQueryResponse>
			("SP_GetCommentById", parameters, commandType: CommandType.StoredProcedure);

		return activity.FirstOrDefault();
	}

	public async Task<ListDto> GetAll(GetAllCommentsFiltering filtering
		, GetAllCommentOrdering ordering, PaginationFilter pagination
		, CancellationToken token)
	{

		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//filtering 
		parameters.Add("projectId", filtering.ProjectId);
		parameters.Add("activityId", filtering.ActivityId);
		parameters.Add("search", filtering.Search);
		parameters.Add("userId", filtering.userId);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


		var comments = await connection.QueryAsync<GetAllCommentsQueryResponse>
		("SP_GetComments", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, comments.ToImmutableList());
	}


	public async Task<Comment?> FindById(long id, CancellationToken token)
	{
		return await _context.Comments
			.FirstOrDefaultAsync(x => x.Id == id, token);
	}

	public async Task<List<Comment>> Find(long? projectId, 
		long? activityId, CancellationToken token)
	{
		return await _context.Comments
			.Where(x => (projectId != null ? x.ProjectId == projectId : true)
			&& (activityId != null ? x.ActivityId == activityId : true))
			.ToListAsync(token);
	}
}
