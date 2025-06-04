using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity;

public record class GetMemberOfActivityQueryRequest(
    string ActivityId

    ): IRequest<List<GetMemberOfActivityQueryResponse>>;
