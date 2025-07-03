using FluentValidation;

namespace Core.Application.ApplicationServices.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(3);

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(8);

        RuleFor(x => x.Category)
            .Must(x => (int)x >= 0 && (int)x <= 1);
    }
}
