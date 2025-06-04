using Core.Application.ApplicationServices.Comments.Queries.GetCommentById;
using Core.Application.ApplicationServices.Projects.Queries.GetProjectById;
using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;

namespace Infrastructure.Repositories
{
    public class ProjectRepository(ApplicationContext context, IConfiguration configuration) : IProjectRepository
    {
        private readonly ApplicationContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        //Commands
        public async Task AddProject(Project project, CancellationToken token)
        {
            await _context.Projects.AddAsync(project, token);
        }

        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
        }

        public void DeleteRangeProject(ICollection<Project> projects)
        {
            _context.Projects.RemoveRange(projects);
        }

        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
        }

        //Queries
        public async Task<IResponse?> GetProjectById(string id, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("projectId", id);

            var comment = await connection.QueryAsync<GetProjectByIdQueryResponse>
                ("SP_GetProjectById", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return comment.FirstOrDefault();
        }
    }
}
