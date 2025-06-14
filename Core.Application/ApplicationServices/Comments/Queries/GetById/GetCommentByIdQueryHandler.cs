using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetById;

public class GetCommentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetCommentByIdQueryRequest, GetCommentByIdQueryResponse>
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<GetCommentByIdQueryResponse> Handle(GetCommentByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var comment = await _unitOfWork.Comments
            .GetById(request.Id, cancellationToken);

        var response = comment.Adapt<GetCommentByIdQueryResponse>();
        return response;
    }
}
