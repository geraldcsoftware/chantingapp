using MediatR;

namespace ChantingApp.Api.ViewModels;

public record EndStreamViewModel(Guid Id) : IRequest<Unit>;