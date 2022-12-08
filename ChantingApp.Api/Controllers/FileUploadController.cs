using ChantingApp.Api.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChantingApp.Api.Controllers;

[ApiController, Route("file-upload")]
public class FileUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<string>> UploadFile(IFormFile file)
    {
        var url = await _mediator.Send(new UploadFileRequest(file));
        return url;
    }
}