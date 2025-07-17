using Core.Application.ApplicationServices.Projects.Queries.GetAll;
using Core.Application.ApplicationServices.Projects.Queries.GetMembers;
using Core.Domain.Entities.ProjectMembers;

namespace Infrastructure.Persistence.Repositories;

public class ProjectMembersRepository(ApplicationContext context,
	IConfiguration configuration
	) : IProjectMembersRepository
{
	private readonly ApplicationContext _context = context;
	private readonly string? _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];

	//Commands
	public ProjectMember Add(ProjectMember projectMember)
	{ 
		return _context.ProjectMembers.Add(projectMember).Entity;
	}

	public void AddRange(ICollection<ProjectMember> projectMember)
	{
		_context.ProjectMembers.AddRange(projectMember);
	}

	public void Remove(ProjectMember projectMember)
	{
		_context.Remove(projectMember);
	}

	public void RemoveRange(ICollection<ProjectMember> projectMembers)
	{
		_context.RemoveRange(projectMembers);
	}

	//Queries
	public async Task<ProjectMember?> FindById(long id, CancellationToken token)
	{
		return await _context.ProjectMembers
			.FirstOrDefaultAsync(p => p.Id == id, token);
	}

	public async Task<IReadOnlyCollection<ProjectMember>> FindByProjectId(long projectId, CancellationToken token)
	{
		var projectMembers = await _context.ProjectMembers
			.Where(x => x.ProjectId == projectId)
			.ToListAsync(token);

		return projectMembers.ToImmutableList();
	}

	public async Task<ProjectMember?> GetByUserIdAndProjectId(Guid userId, long projectId, CancellationToken token)
	{
		var projectMember = await _context.ProjectMembers
			.FirstOrDefaultAsync(x => x.ProjectId == projectId && 
			x.MemberId == userId,
			token);

		return projectMember;
	}

	public async Task<IReadOnlyCollection<Guid>> FindMemberIdsOfProject(long projectId, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("projectId", projectId, DbType.Int64);

		var memberIds = await connection.QueryAsync<Guid>
			("SP_FindMemberIdsOfProject", parameters, commandType: CommandType.StoredProcedure);

		return memberIds.ToImmutableList();
	}

	public async Task<ListDto> GetProjectOfUserId(Guid userId,
		GetAllProjectsFiltering filtering, GetAllProjectsOrdring ordering,
		PaginationFilter pagination, CancellationToken token)
	{
		//GetAllProjectQueryResponse
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		//
		parameters.Add("memberId", userId);
		parameters.Add("isActive", true);
		//filtering
		parameters.Add("startDate", filtering.StartDate);
		parameters.Add("endDate", filtering.EndDate);
		parameters.Add("isOwner", filtering.UserIsOwner);
		//ordering
		parameters.Add("OrderDirection", ordering.GetOrderDirection(ordering));
		parameters.Add("OrderBy", ordering.GetOrderBy(ordering));
		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var projects = await connection.QueryAsync<GetAllProjectQueryResponse>
			("SP_GetProjects", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, projects.ToImmutableList());
	}

	public async Task<bool> IsMemberOfProject(long projectId, Guid userId, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();
		parameters.Add("projectId", projectId, DbType.Int64);
		parameters.Add("memberId", userId, DbType.Guid);
		parameters.Add("isMember", dbType: DbType.Boolean, direction: ParameterDirection.Output);

		 await connection.QueryAsync
			("SP_IsMemberOfProject", parameters, commandType: CommandType.StoredProcedure);

		var isMember = parameters.Get<bool>("isMember");


		return isMember;
	}

	public async Task<ListDto> GetMemberOfProject(long projectId, 
		PaginationFilter pagination, CancellationToken token)
	{
		await using var connection = new SqlConnection(_connectionString);
		await connection.OpenAsync(token);

		var parameters = new DynamicParameters();

		parameters.Add("projectId", projectId);

		//pagination
		parameters.Add("PageNumber", pagination.PageNumber);
		parameters.Add("PageSize", pagination.PageSize);

		parameters.Add("TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

		var members = await connection.QueryAsync<GetMemberOfProjectQueryResponse>
			("SP_GetMembersOfProject", parameters, commandType: CommandType.StoredProcedure);

		var totalCount = parameters.Get<int>("TotalCount");

		return new ListDto(totalCount, members.ToImmutableList());
	}

}
