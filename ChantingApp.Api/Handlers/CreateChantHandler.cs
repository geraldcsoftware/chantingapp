using System.Globalization;
using System.Text.Json;
using ChantingApp.Api.Services;
using ChantingApp.Api.ViewModels;
using ChantingApp.Persistence;
using MediatR;

namespace ChantingApp.Api.Handlers;

public class CreateChantHandler : IRequestHandler<CreateChantViewModel, ChantViewModel>
{
    private readonly ChantsDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly ILogger<CreateChantHandler> _logger;

    public CreateChantHandler(ChantsDbContext dbContext,
                              ICurrentUserContext currentUserContext,
                              ILogger<CreateChantHandler> logger)
    {
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
        _logger = logger;
    }

    public async Task<ChantViewModel> Handle(CreateChantViewModel request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();

        var visualPreset = GetVisualPreset(request.VisualPreset);
        _logger.LogInformation("Chant visual preset: {@VisualPreset}", visualPreset);
        
        if (visualPreset == null) 
            throw new Exception("Invalid visual preset details");

        var chant = new Chant
        {
            Name = request.Name!,
            Description = request.Description!,
            Id = Guid.NewGuid(),
            CreatedByUserId = _currentUserContext.GetUserId()!,
            VisualPreset = new ChantVisualDisplay
            {
                PresetType = visualPreset.PresetType,
                Color = (visualPreset as SingleColourPreset)?.Color,
                BackgroundImageUrl = (visualPreset as BackgroundImagePreset)?.ImageUrl
            }
        };

        _dbContext.Chants.Add(chant);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ChantViewModel
        {
            Name = chant.Name,
            Description = chant.Description,
            Id = chant.Id,
            VisualPreset = new VisualPresetViewModel
            {
                Color = chant.VisualPreset.Color?.ToString(),
                ImageUrl = chant.VisualPreset.BackgroundImageUrl,
                Type = chant.VisualPreset.PresetType.ToString()
            }
        };
    }

    private static ChantVisualPreset? GetVisualPreset(JsonElement? json)
    {
        if (json == null) return null;
        return json.Value switch
        {
            { ValueKind: JsonValueKind.String } => TryGetColorPreset(json.Value.ToString()),
            { ValueKind: JsonValueKind.Object } => TryGetPresetFromJsonObject(json.Value),
            _                                   => throw new InvalidOperationException("Cannot convert the supplied value to any known chant preset type")
        };
    }

    private static ChantVisualPreset? TryGetColorPreset(string colorCode)
    {
        if (Color.TryParse(colorCode, CultureInfo.InvariantCulture, out var color))
        {
            return new SingleColourPreset { Color = color };
        }

        return null;
    }

    private static ChantVisualPreset? TryGetPresetFromJsonObject(JsonElement element)
    {
        ArgumentNullException.ThrowIfNull(element);
        if (element.ValueKind != JsonValueKind.Object) throw new InvalidOperationException($"Expected JsonValueKind 'Object', but got '{element.ValueKind}'");

        if (element.TryGetProperty("color", out var colorProperty))
            return TryGetColorPreset(colorProperty.GetString()!);
        if (element.TryGetProperty("imageUrl", out var imageProperty))
        {
            var imageUrl = imageProperty.GetString();
            return new BackgroundImagePreset { ImageUrl = imageUrl! };
        }

        throw new InvalidOperationException("Unsupported preset type");
    }
}