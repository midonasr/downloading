using MediatR;
namespace StorageService.Application.Commands
{
    public record UploadFileCommand(string Filename, byte[] Content) : IRequest<string>;

}
