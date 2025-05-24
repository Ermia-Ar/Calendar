using Core.Application.DTOs.ActivityDTOs;
using FluentValidation;

namespace Core.Application.Validators
{
    public class CreateActivityRequestValidator : AbstractValidator<CreateActivityRequest>
    {
        public CreateActivityRequestValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(3)
                .NotNull();

            RuleFor(x => x.StartDate)
                .NotNull()
                .GreaterThanOrEqualTo(DateTime.UtcNow - TimeSpan.FromMinutes(3));

            RuleFor(x => x.Category)
                .Must(x => (int)x <= 1);
        }
    }
}
