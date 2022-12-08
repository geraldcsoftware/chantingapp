using ChantingApp.Api.ViewModels;
using FluentValidation;

namespace ChantingApp.Api.Validators;

public class EndStreamViewModelValidator: AbstractValidator<EndStreamViewModel>
{
    public EndStreamViewModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}