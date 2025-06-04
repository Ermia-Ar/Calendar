using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddProject;

public record class AddProjectCommandRequest(
    string Title ,
    string Description ,
    DateTime StartDate ,
    DateTime EndDate ,
    string RequestMassage ,
    string[] Members
    
    ): IRequest;
