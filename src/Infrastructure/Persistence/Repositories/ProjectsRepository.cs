namespace Infrastructure.Persistence.Repositories;

public class ProjectsRepository(ApplicationContext context, IConfiguration configuration) : IProjectsRepository
{
    private readonly ApplicationContext _context = context;
	private readonly string _connectionString = configuration["CONNECTIONSTRINGS:CONNECTION"];


	//Commands
	public Project Add(Project project)
    {
        var result =  _context.Projects.Add(project);
        return result.Entity;
    }

    public void Remove(Project project)
    {
        _context.Projects.Remove(project);
    }

    public void RemoveRange(ICollection<Project> projects)
    {
        _context.Projects.RemoveRange(projects);
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    //Queries
    public async Task<IResponse?> GetById(long id, CancellationToken token)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(token);

        var parameters = new DynamicParameters();
        parameters.Add("projectId", id);

        var project = await connection.QueryAsync<GetProjectByIdQueryResponse>
            ("SP_GetProjectById", parameters, commandType: CommandType.StoredProcedure);

        return project.FirstOrDefault();
    }

    public async Task<Project?> FindById(long id, CancellationToken token)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }
}
