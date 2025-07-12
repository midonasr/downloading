using Google.Protobuf;
using Grpc.Core;
using MediatR;
using StorageService.Api.Protos;
using StorageService.Application.Commands;
using StorageService.Application.Queries;

namespace StorageService.Api.Services;

public class StorageGrpcService : StorageService.Api.Protos.StorageService.StorageServiceBase
{
    private readonly IMediator _mediator;
    public StorageGrpcService(IMediator mediator) => _mediator = mediator;

    public override async Task<UploadReply> Upload(UploadRequest request, ServerCallContext context)
    {
        var id = await _mediator.Send(new UploadFileCommand(request.Filename, request.Content.ToByteArray()));
        return new UploadReply { Id = id };
    }

    public override async Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
    {
        var success = await _mediator.Send(new DeleteFileCommand(request.Id));
        return new DeleteReply { Success = success };
    }

    public override async Task<GetMetadataReply> GetMetadata(GetMetadataRequest request, ServerCallContext context)
    {
        var dto = await _mediator.Send(new GetFileMetadataQuery(request.Id));
        return new GetMetadataReply
        {
            Id = dto.Id,
            Filename = dto.Filename,
            Size = dto.Size,
            UploadedAt = dto.UploadedAt.ToString("o"),
            IsInfected = dto.IsInfected
        };
    }

    public override async Task<DownloadReply> Download(DownloadRequest req, ServerCallContext ctx)
    {
        var (filename, content) = await _mediator.Send(new DownloadFileQuery(req.Id));
        return new DownloadReply
        {
            Filename = filename,
            Content = ByteString.CopyFrom(content)
        };
    }

}
