using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Application.ApplicationServices.Projects.Queries.GetActivities;
using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Helper;
using System.Collections.Immutable;

namespace Infrastructure.Repositories;

public class ActivityRepository(ApplicationContext context,
                                IConfiguration configuration) : IActivitiesRepository
{
    private readonly ApplicationContext _context = context;
    private readonly IConfiguration _configuration = configuration;


    //Commands
    public void Update(Activity UpdateActivity)
    {
        _context.Activities.Update(UpdateActivity);
    }
    public async Task Add(Activity activity, CancellationToken token)
    {
        _context.Activities.Add(activity);
    }

    public void Delete(Activity activity)
    {
        _context.Activities.Remove(activity);
    }

    public void RemoveRange(ICollection<Activity> activities)
    {
        _context.Activities.RemoveRange(activities);
    }
    //Queries
    public async Task<IReadOnlyCollection<IResponse>> GetActivities(string projectId, CancellationToken token
        , DateTime? startDate = null)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("projectId", projectId);
        parameters.Add("startDate", startDate);


        var activities = await connection.QueryAsync<GetActivityOfProjectQueryResponse>(
            "Sp_GetProjectActivities",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure
            );

        return activities.ToImmutableList();
    }

    public async Task<string[]> GetActiveActivitiesId(string projectId, CancellationToken token)
    {

        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("projectId", projectId);
        parameters.Add("startDate", DateTime.Now);


        var activities = await connection.QueryAsync<string>(
            "Sp_GetProjectActiveActivityIds",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure
            );

        return activities.ToArray();
    }


    public async Task<IResponse?> GetById(string id, CancellationToken token)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Connection"));
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("activityId", id);


        var activities = await connection.QueryAsync<GetActivityByIdQueryResponse>(
            "SP_GetActivityById",
            parameters,
            commandType: System.Data.CommandType.StoredProcedure
            );

        return activities.FirstOrDefault();
    }
}
