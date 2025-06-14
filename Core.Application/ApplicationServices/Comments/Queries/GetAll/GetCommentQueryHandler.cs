using AutoMapper;
using Core.Application.Exceptions;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public sealed class GetCommentQueryHandler
    : IRequestHandler<GetAllCommentsQueryRequest, List<GetCommentsQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IMapper _mapper;

    public GetCommentQueryHandler(IMapper mapper, ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _currentUserServices = currentUserServices;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<GetCommentsQueryResponse>> Handle(GetAllCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        //string userId = _currentUserServices.GetUserId();

        //if (request.projectId == null && request.ActivityId == null && !request.UserOwner)
        //{
        //    throw new BadRequestExceptions("bad request");
        //}

        //var comments = await _unitOfWork.Comments.GetComments(request.projectId, request.ActivityId
        //  , request.Search, request.UserOwner ? userId : null, cancellationToken);

        //var response = _mapper.Map<List<GetCommentsQueryResponse>>(comments);
        //return response;
        throw new NotImplementedException();
    }
}
