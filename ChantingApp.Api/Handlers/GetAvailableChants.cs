using ChantingApp.Api.ViewModels;
using ChantingApp.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChantingApp.Api.Handlers;

public class GetAvailableChantsRequest : IRequest<IReadOnlyCollection<ChantViewModel>>
{
}

public class GetAvailableChantsHandler : IRequest<IReadOnlyCollection<ChantViewModel>>,
                                         IRequestHandler<GetAvailableChantsRequest, IReadOnlyCollection<ChantViewModel>>
{
    private readonly ChantsDbContext _dbContext;

    public GetAvailableChantsHandler(ChantsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ChantViewModel>> Handle(GetAvailableChantsRequest request, CancellationToken cancellationToken)
    {
        var chants = await _dbContext.Chants.Include(c => c.Stream)
                                     .Where(c => c.Stream != null &&
                                                 c.Stream.EndTime == null &&
                                                 c.Stream.Status == StreamStatus.Live)
                                     .ToListAsync(cancellationToken);
        return chants.Select(c => new ChantViewModel
        {
            Name = c.Name,
            Description = c.Description,
            Id = c.Id,
            VisualPreset = new VisualPresetViewModel
            {
                Color = c.VisualPreset.Color?.ToString(),
                ImageUrl = c.VisualPreset.BackgroundImageUrl,
                Type = c.VisualPreset.PresetType.ToString()
            }
        }).ToList();
    }
}