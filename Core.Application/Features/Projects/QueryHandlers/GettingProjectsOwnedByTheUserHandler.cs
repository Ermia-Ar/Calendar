using AutoMapper;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GettingProjectsOwnedByTheUserHandler : ResponseHandler
        , IRequestHandler<GettingProjectsOwnedByTheUserQuery, Response<List<ProjectResponse>>>
    {

        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public GettingProjectsOwnedByTheUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<List<ProjectResponse>>> Handle(GettingProjectsOwnedByTheUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var projects = await _unitOfWork.Projects
                .GetProjectsOwnedByTheUser(userId, cancellationToken, null);

            var response = _mapper.Map<List<ProjectResponse>>(projects);
            return Success(response);
        }
    }
}
