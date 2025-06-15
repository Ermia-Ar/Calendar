using AutoMapper;
using Core.Application.Exceptions;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public sealed class GetAllCommentQueryHandler
    : IRequestHandler<GetAllCommentsQueryRequest, List<GetAllCommentsQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IMapper _mapper;

    public GetAllCommentQueryHandler(IMapper mapper, ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _currentUserServices = currentUserServices;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<GetAllCommentsQueryResponse>> Handle(GetAllCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUserServices.GetUserId();

        if (request.Filtering.ProjectId == null && request.Filtering.ActivityId == null && !request.Filtering.UserOwner)
        {
            throw new BadRequestExceptions("bad request");
        }

        var comments = await _unitOfWork.Comments.GetAll(request.Filtering.ProjectId, request.Filtering.ActivityId
          , request.Filtering.Search, request.Filtering.UserOwner ? userId : null, cancellationToken);

        var response = _mapper.Map<List<GetAllCommentsQueryResponse>>(comments);
        return response;
    }
}
