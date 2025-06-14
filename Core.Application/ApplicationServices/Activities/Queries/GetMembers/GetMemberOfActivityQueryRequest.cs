using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

public record class GetMemberOfActivityQueryRequest(
    string ActivityId

    ): IRequest<List<GetMemberOfActivityQueryResponse>>;
