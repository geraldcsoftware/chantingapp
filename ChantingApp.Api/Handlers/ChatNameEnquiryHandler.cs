using ChantingApp.Api.ViewModels;
using ChantingApp.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChantingApp.Api.Handlers;

public class ChatNameEnquiryHandler : IRequestHandler<ChatNameEnquiryRequest, ChantViewModel?>
{
    private readonly ChantsDbContext _dbContext;

    public ChatNameEnquiryHandler(ChantsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChantViewModel?> Handle(ChatNameEnquiryRequest request, CancellationToken cancellationToken)
    {
        var chant = await _dbContext.Chants.Include(c => c.Stream)
                                    .Where(c => c.Name == request.Name)
                                    .Where(c => c.Stream != null &&
                                                c.Stream.EndTime == null &&
                                                c.Stream.Status == StreamStatus.Live)
                                    .FirstOrDefaultAsync(cancellationToken);
        return chant == null
                   ? null
                   : new ChantViewModel
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
}