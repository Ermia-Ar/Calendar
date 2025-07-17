using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.ProjectMembers;
using Core.Domain.Entities.Projects;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public class AddProjectCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserServices)
        : IRequestHandler<AddProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(AddProjectCommandRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var ownerId = _currentUserServices.GetUserId();

            //map to project entity
            var project = ProjectFactory.Create(ownerId, request.Title,
                request.Description, request.StartDate, request.EndDate,
                request.Icon, request.Color);

            project = _unitOfWork.Projects.Add(project);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var projectMembers = new List<ProjectMember>();

            //add the owner of project to the members 
            var projectOwner = ProjectMember
                .Create(ownerId, project.Id);

            projectMembers.Add(projectOwner);

            foreach (var memberId in request.MemberIds)
            {
                //check
                var member = (await _unitOfWork
                        .Users.GetById(memberId, cancellationToken))
                    .Adapt<GetUserByIdDto>();

                if (member == null)
                    throw new NotFoundUserIdException(memberId);

                var projectMember = ProjectMember
                    .Create(memberId, project.Id);

                projectMembers.Add(projectMember);
            }

            _unitOfWork.ProjectMembers.AddRange(projectMembers);
            //save changes
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RoleBackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
