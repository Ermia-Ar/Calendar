using FluentValidation;

namespace Core.Application.ApplicationServices.Projects.Commands.AddMembers;

public class AddMemberToProjectCommandValidator : AbstractValidator<AddMembersToProjectCommandRequest>
{
	public AddMemberToProjectCommandValidator()
	{
		RuleFor(x => x.Message)
			.NotEmpty();
	}
}
