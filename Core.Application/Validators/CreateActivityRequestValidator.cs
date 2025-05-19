using Core.Application.DTOs.ActivityDTOs;
using FluentValidation;

namespace Core.Application.Validators
{
    public class CreateActivityRequestValidator : AbstractValidator<CreateActivityRequest>
    {
        public CreateActivityRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.StartDate)
                .NotNull()
                .GreaterThanOrEqualTo(DateTime.UtcNow - TimeSpan.FromSeconds(30));

            RuleFor(x => x.DurationInMinute)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Category)
                .IsInEnum();
        }
    }
}
