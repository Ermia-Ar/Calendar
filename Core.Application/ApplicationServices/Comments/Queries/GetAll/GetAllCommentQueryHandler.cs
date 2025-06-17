using AutoMapper;
using Core.Application.Exceptions;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public sealed class GetAllCommentQueryHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        : IRequestHandler<GetAllCommentsQueryRequest, List<GetAllCommentsQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetAllCommentsQueryResponse>> Handle(GetAllCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUserServices.GetUserId();

        if (request.Filtering.ProjectId == null && request.Filtering.ActivityId == null && !request.Filtering.UserOwner)
        {
            throw new BadRequestExceptions("bad request");
        }

        var comments = await _unitOfWork.Comments.GetAll(request.Filtering.ProjectId, request.Filtering.ActivityId
          , request.Filtering.Search, request.Filtering.UserOwner ? userId : null, cancellationToken);

        var response = comments.Adapt<List<GetAllCommentsQueryResponse>>();
        return response;
    }
}
