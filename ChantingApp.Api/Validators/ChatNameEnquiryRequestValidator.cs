using ChantingApp.Api.ViewModels;
using FluentValidation;

namespace ChantingApp.Api.Validators;

public class ChatNameEnquiryRequestValidator : AbstractValidator<ChatNameEnquiryRequest>
{
    public ChatNameEnquiryRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .MinimumLength(3);
    }
}