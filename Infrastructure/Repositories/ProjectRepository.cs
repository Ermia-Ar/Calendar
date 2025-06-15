using Core.Application.ApplicationServices.Projects.Queries.GetById;
using Core.Domain.Entity.Projects;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;

namespace Infrastructure.Repositories
{
    public class ProjectRepository(ApplicationContext context, IConfiguration configuration) : IProjectsRepository
    {
        private readonly ApplicationContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        //Commands
        public void Add(Project project)
        {
            _context.Projects.Add(project);
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
        public async Task<IResponse?> GetById(string id, CancellationToken token)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
            await connection.OpenAsync(token);

            var parameters = new DynamicParameters();
            parameters.Add("projectId", id);

            var comment = await connection.QueryAsync<GetProjectByIdQueryResponse>
                ("SP_GetProjectById", parameters, commandType: System.Data.CommandType.StoredProcedure);

            return comment.FirstOrDefault();
        }

        public async Task<Project?> FindById(string id, CancellationToken token)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }
    }
}
