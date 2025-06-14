using Core.Application.ApplicationServices.Activities.Queries.GetById;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivities
{
    public record class GetActivitiesOfProjectQueryRequest(string ProjectId)
        : IRequest<List<GetActivityOfProjectQueryResponse>>;
}
