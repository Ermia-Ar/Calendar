using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetProjectById
{
    public sealed class GetProjectByIdQueryHandler
        : IRequestHandler<GetProjectByIdQueryRequest, GetProjectByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;

        public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<GetProjectByIdQueryResponse> Handle(GetProjectByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();

            var isMember = (await _unitOfWork.Requests.GetMemberOfProject(request.Id, cancellationToken))
                .Adapt<List<User>>()
                .Any(x => x.Id == userId);

            if (!isMember)
            {
                throw new OnlyProjectMembersAllowedException();
            }

            var project = await _unitOfWork.Projects
                .GetProjectById(request.Id, cancellationToken);

            var response = project.Adapt<GetProjectByIdQueryResponse>();

            return response;
        }
    }
}
