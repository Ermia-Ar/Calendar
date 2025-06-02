using AutoMapper;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.Exceptions.Project;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using MediatR;
using OfficeOpenXml.Drawing;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public sealed class GetProjectByIdHandler
        : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;

        public GetProjectByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<ProjectResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var project = await _unitOfWork.Projects.GetProjectById(request.Id, cancellationToken);

            var isMember = (await _unitOfWork.Requests.GetMemberOfProject(request.Id, cancellationToken))
                .Any(x => x.Id == userId);

            if (!isMember)
            {
                throw new OnlyProjectMembersAllowedException();
            }
            var response  = _mapper.Map<ProjectResponse>(project);

            return response;
        }
    }
}
