using MediatR;

namespace ChantingApp.Api.ViewModels;

public class ChatNameEnquiryRequest : IRequest<ChantViewModel?>
{
    public string? Name { get; set; }
}