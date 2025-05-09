using AutoMapper;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class CreateProjectHandler : ResponseHandler
        , IRequestHandler<CreateProjectCommand, Response<string>>
    {
        private IUnitOfWork _unitOfWork;
        private ICurrentUserServices _currentUserServices;
        private IMapper _mapper;

        public CreateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // check UserNames exist ?
            foreach (var Receiver in request.CreateProject.Members)
            {
                var isExist = await _unitOfWork.Users.IsUserNameExist(Receiver);
                if (!isExist)
                {
                    return NotFound<string>($"user name {Receiver} does not exist !");
                }
            }
            //map to project entity
            var project = _mapper.Map<Project>(request.CreateProject);
            project.Id = Guid.NewGuid().ToString();
            project.OwnerId = _currentUserServices.GetUserId();
            project.CreatedDate = DateTime.Now;
            project.UpdateDate = DateTime.Now;
            // add to project table
            await _unitOfWork.Projects.AddAsync(project, cancellationToken);

            //sent request for each members
            var userRequests = new List<UserRequest>();
            foreach (var Receiver in request.CreateProject.Members)
            {
                var sendRequest = new UserRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    InvitedAt = DateTime.Now,
                    Status = Domain.Enum.RequestStatus.Pending,
                    Sender = _currentUserServices.GetUserName(),
                    IsGuest = false,
                    IsExpire = false,
                    IsActive = true,
                    Receiver = Receiver,
                    Message = request.CreateProject.RequestMassage,
                    RequestFor = Domain.Enum.RequestFor.Project,
                    ProjectId = project.Id,
                };
                userRequests.Add(sendRequest);
            }
            await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();
        }
    }
}
