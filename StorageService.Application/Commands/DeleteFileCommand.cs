using MediatR;

namespace StorageService.Application.Commands
{
    public record DeleteFileCommand(string Id) : IRequest<bool>;
}

