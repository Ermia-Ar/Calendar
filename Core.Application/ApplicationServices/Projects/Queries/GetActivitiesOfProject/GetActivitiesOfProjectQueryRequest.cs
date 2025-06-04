using Core.Application.ApplicationServices.Activities.Queries.GetById;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivitiesOfProject
{
    public record class GetActivitiesOfProjectQueryRequest(string ProjectId)
        : IRequest<List<GetActivityByIdQueryResponse>>;
}
