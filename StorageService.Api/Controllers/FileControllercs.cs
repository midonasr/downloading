using Microsoft.AspNetCore.Mvc;
using MediatR;
using StorageService.Application.Commands;
using StorageService.Application.Queries;

namespace StorageService.Api.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest model)
    {
        _logger.LogInformation("Upload requested for file {FileName} at {Time}",
        model.File?.FileName, DateTime.UtcNow);
        if (model.File == null || model.File.Length == 0)
        {
            _logger.LogWarning("Invalid file upload attempt at {Time}", DateTime.UtcNow);

            return BadRequest("File is required");
        }
        using var ms = new MemoryStream();
        await model.File.CopyToAsync(ms);

        var id = await _mediator.Send(new UploadFileCommand(model.File.FileName, ms.ToArray()));
        _logger.LogInformation("Successfully uploaded {FileName}, size {Size} bytes",
               model.File.FileName, model.File.Length);
        return Ok(new { id });
    }

    [HttpGet("metadata/{id}")]
    public async Task<IActionResult> GetMetadata(string id)
    {
        var metadata = await _mediator.Send(new GetFileMetadataQuery(id));
        return Ok(metadata);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var success = await _mediator.Send(new DeleteFileCommand(id));
        return Ok(new { success });
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(string id)
    {
        try
        {

            var (filename, content) = await _mediator.Send(new DownloadFileQuery(id));
            _logger.LogInformation("Download requested for file {filename}", filename);

            return File(content, "application/octet-stream", filename);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Requested file not found");
            return NotFound();
        }
    }

}
