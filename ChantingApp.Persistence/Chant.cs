namespace ChantingApp.Persistence;

public class Chant
{
    public Guid Id { get; set; }
    public required string CreatedByUserId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ChantVisualDisplay VisualPreset { get; set; }
    public ChantStream? Stream { get; set; }
}

public class ChantVisualDisplay
{
    public ChantVisualPresetType PresetType { get; set; }
    public Color? Color { get; set; }
    public IReadOnlyCollection<Color>? Colors { get; set; }
    public string? BackgroundImageUrl { get; set; }
}

public abstract class ChantVisualPreset
{
    public ChantVisualPresetType PresetType { get; set; }
}

public enum ChantVisualPresetType
{
    SingleColor,
    BackgroundImage,
    MultiColor
}

public class BackgroundImagePreset : ChantVisualPreset
{
    public required string ImageUrl { get; set; }
}

public class SingleColourPreset : ChantVisualPreset
{
    public required Color Color { get; set; }
}

public class MultiColorPreset : ChantVisualPreset
{
    public required IReadOnlyCollection<Color> Colors { get; set; }
}