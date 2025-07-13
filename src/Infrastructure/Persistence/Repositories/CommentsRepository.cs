
using Core.Activities.ApplicationServices.Activities.Queries.GetComments;
using Core.Application.ApplicationServices.Projects.Queries.GetComments;
using Core.Domain.Entities.Projects;

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

	public async Task<ListDto> GetByActivityId(long activityId, GetActivityCommentsFiltering filtering
		, GetActivityCommentsOrdering ordering, PaginationFilter pagination
		, CancellationToken token)
	{

		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();

		parameters.Add("activityId", activityId);
		//filtering 
		parameters.Add("search", filtering.Search);
		parameters.Add("userId", filtering.userId);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


		var comments = await connection.QueryAsync<GetActivityCommentsQueryResponse>
		("SP_GetCommentsByActivityId", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, comments.ToImmutableList());
	}

	public async Task<ListDto> GetByProjectId(long projectId,
		GetProjectCommentsFiltering filtering, GetProjectCommentsOrdering ordering, 
		PaginationFilter pagination, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("projectId", projectId);
		//filtering 
		parameters.Add("activityId", filtering.ActivityId);
		parameters.Add("search", filtering.Search);
		parameters.Add("userId", filtering.UserId);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


		var comments = await connection.QueryAsync<GetProjectCommentsQueryResponse>
		("SP_GetCommentsByProjectId", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, comments.ToImmutableList());
	}

	public async Task<ListDto> GetByUserId(Guid userId, GetUserCommentsFiltering filtering,
		GetUserCommentsOrdering ordering, PaginationFilter pagination,
		CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("userId", userId);
		//filtering 
		parameters.Add("activityId", filtering.ActivityId);
		parameters.Add("projectId", filtering.ProjectId);
		parameters.Add("search", filtering.Search);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


		var comments = await connection.QueryAsync<GetProjectCommentsQueryResponse>
		("SP_GetCommentsByUserId", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, comments.ToImmutableList());
	}

	public async Task<Comment?> FindById(long id, CancellationToken token)
	{
		return await _context.Comments
			.FirstOrDefaultAsync(x => x.Id == id, token);
	}
}
