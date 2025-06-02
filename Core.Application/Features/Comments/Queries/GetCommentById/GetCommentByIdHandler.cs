using AutoMapper;
using Core.Application.Features.Comments.Queries.GetComments;
using Core.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Features.Comments.Queries.GetCommentById
{
    public class GetCommentByIdHandler
        : IRequestHandler<GetCommentByIdQuery, GetCommentsResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;

        public GetCommentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<GetCommentsResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.Comments
                .GetCommentById(request.Id, cancellationToken);

            var response = _mapper.Map<GetCommentsResponse>(comment);
            return response;
        }
    }
}
