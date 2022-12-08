namespace ChantingApp.Api.ViewModels;

public class ChantViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required VisualPresetViewModel VisualPreset { get; set; }
}

public class VisualPresetViewModel
{
    public required string Type { get; set; }
    public string? Color { get; set; }
    public IReadOnlyCollection<string>? Colors { get; set; }
    public string? ImageUrl { get; set; }
}