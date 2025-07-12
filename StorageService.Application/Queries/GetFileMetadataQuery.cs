using MediatR;
using StorageService.Domain.Entities;

namespace StorageService.Application.Queries
{
    public record GetFileMetadataQuery(string Id) : IRequest<FileMetadata>;

}
