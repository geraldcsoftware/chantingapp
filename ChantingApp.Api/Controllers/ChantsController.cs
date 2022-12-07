using ChantingApp.Api.Handlers;
using ChantingApp.Api.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChantingApp.Api.Controllers;

[ApiController, Route("chants")]
public class ChantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ChantViewModel>> CreateChant(CreateChantViewModel model, CancellationToken ct)
    {
        var chant = await _mediator.Send(model, ct);
        return chant;
    }

    [HttpPost("{id:guid}/end-stream")]
    public async Task<IActionResult> EndChantStream(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new EndStreamViewModel(id), ct);
        return NoContent();
    }
    

    [HttpGet]
    public async Task<IEnumerable<ChantViewModel>> GetAvailableChants(CancellationToken ct)
    {
        var chants = await _mediator.Send(new GetAvailableChantsRequest(), ct);
        return chants;
    }

    [HttpGet("check-name")]
    public async Task<ActionResult<ChantViewModel>> CheckNameAvailability([FromQuery] ChatNameEnquiryRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result switch
        {
            null => NoContent(),
            _    => Ok(result)
        };
    }
}