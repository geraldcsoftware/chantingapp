using Azure.Identity;
using Azure.Storage.Blobs;
using MediatR;

namespace ChantingApp.Api.Handlers;

public record UploadFileRequest(IFormFile File) : IRequest<string>;

public class UploadImageHandler : IRequestHandler<UploadFileRequest, string>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UploadImageHandler> _logger;

    public UploadImageHandler(IConfiguration configuration,
                              ILogger<UploadImageHandler> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> Handle(UploadFileRequest request, CancellationToken cancellationToken)
    {
        return await UploadFile(request.File);
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient("chant-background-images");

        var fileName = Path.Combine(Guid.NewGuid().ToString(), file.FileName);
        var blobClient = containerClient.GetBlobClient(fileName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream);
        var fileUrl = blobClient.Uri.AbsoluteUri;
        return fileUrl;
    }

    
}