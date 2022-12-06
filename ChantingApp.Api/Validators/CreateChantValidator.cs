using ChantingApp.Api.ViewModels;
using FluentValidation;

namespace ChantingApp.Api.Validators;

public class CreateChantValidator:AbstractValidator<CreateChantViewModel>
{
    public CreateChantValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Description).MaximumLength(400);
        RuleFor(c => c.VisualPreset).NotNull();
    }
}