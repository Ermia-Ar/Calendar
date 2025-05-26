using AutoMapper;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetUserProjectsHandler : ResponseHandler
        , IRequestHandler<GetUserProjectsQuery, Response<List<ProjectResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProjectsHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
            _mapper = mapper;
        }

        public async Task<Response<List<ProjectResponse>>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserServices.GetUserId();
            
            var projects = await _unitOfWork.Requests.GetProjects
                (ownerId, request.UserIsOwner, cancellationToken, request.StartDate, request.IsHistory);

            var response = _mapper.Map<List<ProjectResponse>>(projects);
            return Success(response);
        }
    }
}
