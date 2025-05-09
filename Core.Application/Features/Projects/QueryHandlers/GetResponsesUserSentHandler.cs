using AutoMapper;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.Projects.Query;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.QueryHandlers
{
    public class GetResponsesUserSentHandler : ResponseHandler
        , IRequestHandler<GetProjcetResponsesUserSentQuery, Response<List<ProjectRequestResponse>>>
    {
        private ICurrentUserServices _currentUserServices;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetResponsesUserSentHandler(IUnitOfWork unitOfWork, IMapper mapper
            , ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<List<ProjectRequestResponse>>> Handle(GetProjcetResponsesUserSentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // get requests with user name 
                var userName = _currentUserServices.GetUserName();
                var requests = await _unitOfWork.Requests.GetResponsesUserSent(userName, cancellationToken);
                var response = _mapper.Map<List<ProjectRequestResponse>>(requests);
                return Success(response);
            }
            catch
            {
                return BadRequest<List<ProjectRequestResponse>>("Something wrong");
            }
        }
    }
}
