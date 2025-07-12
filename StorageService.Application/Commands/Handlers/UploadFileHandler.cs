using MediatR; 
using StorageService.Application.IRepository;
using StorageService.Application.IServices;
using StorageService.Domain.Entities;

namespace StorageService.Application.Commands.Handlers
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly IStorageService _storage;
        private readonly IFileScannerService _scanner;
        private readonly IFileMetadataRepository _repo;

        public UploadFileHandler(
            IStorageService storage,
            IFileScannerService scanner,
            IFileMetadataRepository repo)
        {
            _storage = storage;
            _scanner = scanner;
            _repo = repo;
        }

        public async Task<string> Handle(UploadFileCommand req, CancellationToken ct)
        {
            if (await _scanner.IsFileInfected(req.Content))
                throw new InvalidOperationException("File is infected.");

            var id = await _storage.UploadAsync(req.Filename, req.Content);

            await _repo.SaveAsync(new FileMetadata
            {
                Id = id,
                Filename = req.Filename,
                Size = req.Content.LongLength,
                IsInfected = false
            });

            return id;
        }
    }

}
