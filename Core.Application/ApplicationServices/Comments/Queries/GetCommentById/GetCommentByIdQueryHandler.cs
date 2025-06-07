using AutoMapper;
using Core.Application.ApplicationServices.Comments.Queries.GetComments;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler
        : IRequestHandler<GetCommentByIdQueryRequest, GetCommentByIdQueryResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;

        public GetCommentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<GetCommentByIdQueryResponse> Handle(GetCommentByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Comments
                .GetCommentById(request.Id, cancellationToken);

            var response = comment.Adapt<GetCommentByIdQueryResponse>();
            return response;
        }
    }
}
