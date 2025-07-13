using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ChangeColor;

public sealed record ChangeColorCommandRequest(
    long ProjectId,
    string Color
) : IRequest
{
    public static ChangeColorCommandRequest Create(long projectId, ChangeColorDto model)
        => new ChangeColorCommandRequest(projectId, model.Color);
}


/// <summary>
/// 
/// </summary>
/// <param name="Color"></param>
public sealed record ChangeColorDto(
    string Color
    );