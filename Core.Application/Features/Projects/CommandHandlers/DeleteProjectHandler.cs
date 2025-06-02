using AutoMapper;
using Core.Application.Exceptions.Project;
using Core.Application.Features.Projects.Command;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Projects.CommandHandlers
{
    public class DeleteProjectHandler 
        : IRequestHandler<DeleteProjectCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public DeleteProjectHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserServices = currentUserServices;
        }

        public async Task<string> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetProjectById(request.ProjcetId, cancellationToken);
            if (project.OwnerId != _currentUserServices.GetUserId())
            {
                throw new OnlyProjectCreatorAllowedException();
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // delete from comments table
                var comments = await _unitOfWork.Comments.GetComments
                    (request.ProjcetId, null, null, null, cancellationToken);

                _unitOfWork.Comments.DeleteRangeComment(comments);

                // delete all request for this project 
                var requests = await _unitOfWork.Requests
                    .GetRequests(request.ProjcetId, null, cancellationToken);

                _unitOfWork.Requests.DeleteRangeRequests(requests);

                // delete all activity for this project 
                var activities = await _unitOfWork.Activities
                    .GetProjectActivities(request.ProjcetId, cancellationToken);

                _unitOfWork.Activities.DeleteRangeActivities(activities);

                // delete from projects table
                _unitOfWork.Projects.DeleteProject(project);

                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return "Deleted";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("something wrong!");
            }
        }
    }
}