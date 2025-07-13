using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ChangeIcon;

public record ChangeIconCommandRequest(
    long ProjectId,
    string Icon
) : IRequest
{
    public static ChangeIconCommandRequest Create(long projectId, ChangeIconDto model)
        => new ChangeIconCommandRequest(projectId, model.Icon); 
}
    
    
    
/// <summary>
/// 
/// </summary>
/// <param name="Icon"></param>
public sealed record ChangeIconDto(
    string Icon
);