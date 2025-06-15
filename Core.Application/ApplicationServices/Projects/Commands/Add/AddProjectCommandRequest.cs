using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Add;

public record class AddProjectCommandRequest(
    string Title ,
    string Description ,
    DateTime StartDate ,
    DateTime EndDate ,
    string Massage ,
    string[] MemberIds
    
    ): IRequest;
