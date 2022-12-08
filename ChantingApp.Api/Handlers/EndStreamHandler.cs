using ChantingApp.Api.Hubs;
using ChantingApp.Api.ViewModels;
using ChantingApp.Persistence;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChantingApp.Api.Handlers;

public class EndStreamHandler : IRequestHandler<EndStreamViewModel, Unit>
{
    private readonly IHubContext<ChantStreamHub> _streamHub;
    private readonly ChantsDbContext _dbContext;
    private readonly ILogger<EndStreamHandler> _logger;

    public EndStreamHandler(IHubContext<ChantStreamHub> streamHub,
                            ChantsDbContext dbContext,
                            ILogger<EndStreamHandler> logger)
    {
        _streamHub = streamHub;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(EndStreamViewModel request, CancellationToken cancellationToken)
    {
        var name = await _dbContext.Chants
                                   .Where(c => c.Id == request.Id)
                                   .Select(c => c.Name)
                                   .FirstOrDefaultAsync(cancellationToken);
        if (string.IsNullOrEmpty(name))
        {
            _logger.LogInformation("Chant `{Id}` not found", request.Id);
            return Unit.Value;
        }

        await _streamHub.Clients.All.SendAsync("StreamEnded", name, cancellationToken: cancellationToken);
        await _dbContext.Streams
                        .Where(s => s.ChantId == request.Id && s.Status == StreamStatus.Live)
                        .ExecuteUpdateAsync(update => update.SetProperty(s => s.EndTime, DateTimeOffset.UtcNow)
                                                            .SetProperty(s => s.Status, StreamStatus.Ended),
                                            cancellationToken: cancellationToken);
        return Unit.Value;
    }
}