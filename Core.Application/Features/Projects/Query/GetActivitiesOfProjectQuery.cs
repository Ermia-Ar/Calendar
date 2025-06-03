using Core.Application.ApplicationServices.Activities.Queries.GetById;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetActivitiesOfProjectQuery(string ProjectId)
        : IRequest<List<GetByIdActivityQueryResponse>>;
}
