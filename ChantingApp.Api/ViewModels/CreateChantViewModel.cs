using System.Text.Json;
using MediatR;

namespace ChantingApp.Api.ViewModels;

public class CreateChantViewModel: IRequest<ChantViewModel>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public JsonElement? VisualPreset { get; set; }
}