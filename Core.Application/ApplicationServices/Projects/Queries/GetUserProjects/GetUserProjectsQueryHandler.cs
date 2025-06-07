using AutoMapper;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetUserProjects
{
    public class GetUserProjectsQueryHandler
        : IRequestHandler<GetUserProjectsQueryRequest, List<GetUserProjectQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProjectsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
            _mapper = mapper;
        }

        public async Task<List<GetUserProjectQueryResponse>> Handle(GetUserProjectsQueryRequest request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserServices.GetUserId();

            var projects = await _unitOfWork.Requests.GetProjects
                (ownerId, request.UserIsOwner, cancellationToken, request.StartDate, request.IsHistory);

            var response = projects.Adapt<List<GetUserProjectQueryResponse>>();
            return response;
        }
    }
}
