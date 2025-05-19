using AutoMapper;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetAllUserProjectHandler : ResponseHandler
        , IRequestHandler<GetAllUserProjectQuery, Response<List<ProjectResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllUserProjectHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
            _mapper = mapper;
        }

        public async Task<Response<List<ProjectResponse>>> Handle(GetAllUserProjectQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var userName = _currentUserServices.GetUserName();

            var projects = await _unitOfWork.Projects
                .GetProjectsOwnedByTheUser(userId, cancellationToken , 
                request.StartDate, request.IsHistory);

            if(!request.UserIsOwner)
            {
                projects.AddRange(await _unitOfWork.Requests
                    .GetProjectsThatTheUserIsMemberOf(userName, cancellationToken
                    , request.StartDate, request.IsHistory));
            }

            var response = _mapper.Map<List<ProjectResponse>>(projects);
            return Success(response);
        }
    }
}
