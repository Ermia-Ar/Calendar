using Core.Application.DTOs.ActivityDTOs;
using FluentValidation;

namespace Core.Application.Validators
{
    public class UpdateActivityRequestValidator : AbstractValidator<UpdateActivityRequest>
    {
        public UpdateActivityRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out var result));

            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Date)
                .NotNull()
                .GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.DurationInMinute)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Category)
                .IsInEnum();
        }
    }
}
