using System.Globalization;
using System.Text.Json;
using ChantingApp.Api.ViewModels;
using ChantingApp.Persistence;
using FluentValidation;
using FluentValidation.Validators;

namespace ChantingApp.Api.Validators;

public class CreateChantValidator : AbstractValidator<CreateChantViewModel>
{
    public CreateChantValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Description).MaximumLength(400);
        RuleFor(c => c.VisualPreset).NotNull()
                                    .SetValidator(new VisualPresetJsonValidator())
                                    .WithMessage("Invalid {PropertyName}. Value must be a json object with one of \"color\", \"colors\", or \"imageUrl\" properties.");
    }
}

public class VisualPresetJsonValidator : PropertyValidator<CreateChantViewModel, JsonElement>
{
    public override bool IsValid(ValidationContext<CreateChantViewModel> context, JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.String => IsValidColor(value),
            JsonValueKind.Object => IsValidPresetObject(value),
            _                    => false
        };
    }

    public override string Name => nameof(VisualPresetJsonValidator);

    private static bool IsValidColor(JsonElement value)
    {
        var colorString = value.GetString();
        return !string.IsNullOrEmpty(colorString)
               && Color.TryParse(colorString, CultureInfo.InvariantCulture, out _);
    }

    private static bool IsValidPresetObject(JsonElement value)
    {
        if (value.TryGetProperty("color", out var colorProperty))
            return IsValidColor(colorProperty);

        if (value.TryGetProperty("colors", out var colorArrayProperty))
        {
            return colorArrayProperty.ValueKind == JsonValueKind.Array
                   && colorArrayProperty.EnumerateArray()
                                        .All(val => IsValidColor(val));
        }

        if (value.TryGetProperty("imageUrl", out var imageUrlProperty))
            return !string.IsNullOrEmpty(imageUrlProperty.GetString());

        return false;
    }
}