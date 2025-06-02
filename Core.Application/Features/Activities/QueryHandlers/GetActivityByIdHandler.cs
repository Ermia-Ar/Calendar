using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Exceptions.Activity;
using Core.Application.Features.Activities.Queries;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Interfaces.Repositories;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public sealed class GetActivityByIdHandler
        : IRequestHandler<GetActivityByIdQuery, ActivityResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserServices _currentUserServices;

        public GetActivityByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<ActivityResponse> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var activity = await _unitOfWork.Activities.GetActivityById(request.Id, cancellationToken);

            var isMember = (await _unitOfWork.Requests.GetMemberOfActivity(request.Id, cancellationToken))
                .Any(x => x.Id == userId);

            if (!isMember)
            {
                throw new OnlyActivityMembersAllowedException();
            }

            var response = _mapper.Map<ActivityResponse>(activity);
            return response;
        }
    }
}
