//using AutoMapper;
//using Core.Application.DTOs.ActivityDTOs;
//using Core.Application.Features.Activities.Queries;
//using Core.Domain.Shared;
//using Core.Domain;
//using MediatR;

//namespace Core.Application.Features.Activities.QueryHandlers
//{
//    public class GetActivitiesThatTheUserIsMemberOfHandler : ResponseHandler
//        , IRequestHandler<GetActivitiesThatTheUserIsMemberOfQuery, Response<List<ActivityResponse>>>
//    {
//        public readonly IUnitOfWork _unitOfWork;
//        public readonly ICurrentUserServices _currentUser;
//        public readonly IMapper _mapper;

//        public GetActivitiesThatTheUserIsMemberOfHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _currentUser = currentUser;
//            _mapper = mapper;
//        }

//        public async Task<Response<List<ActivityResponse>>> Handle(GetActivitiesThatTheUserIsMemberOfQuery request, CancellationToken cancellationToken)
//        {
//            var userName = _currentUser.GetUserName();
//            //get activities 
//            var activities = await _unitOfWork.Requests.GetActivities(userName, cancellationToken,null,null);
//            var response = _mapper.Map<List<ActivityResponse>>(activities);

//            return Success(response);
//        }
//    }
//}