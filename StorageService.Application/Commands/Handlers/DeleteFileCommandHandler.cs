using MediatR; 
using StorageService.Application.IRepository;
using StorageService.Application.IServices;

namespace StorageService.Application.Commands.Handlers
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, bool>
    {
        private readonly IStorageService _storage;
        private readonly IFileMetadataRepository _repo;

        public DeleteFileCommandHandler(
            IStorageService storage,
            IFileMetadataRepository repo)
        {
            _storage = storage;
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var deletedFromS3 = await _storage.DeleteAsync(request.Id);
            if (!deletedFromS3)
                return false;

            // Optionally remove metadata
            await _repo.DeleteByIdAsync(request.Id);
            return true;
        }
    }
}
