 
using MediatR;
using StorageService.Application.IRepository;
using StorageService.Domain.Entities; 

namespace StorageService.Application.Queries.Handlers
{
    public class GetFileMetadataQueryHandler : IRequestHandler<GetFileMetadataQuery, FileMetadata>
    {
        private readonly IFileMetadataRepository _repo;

        public GetFileMetadataQueryHandler(IFileMetadataRepository repo)
        {
            _repo = repo;
        }

        public Task<FileMetadata> Handle(GetFileMetadataQuery req, CancellationToken ct)
        {
            return _repo.GetByIdAsync(req.Id);
        }
    }

}
