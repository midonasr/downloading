using MediatR;
using StorageService.Application.IServices;

namespace StorageService.Application.Queries.Hamdlers
{
    public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, (string Filename, byte[] Content)>
    {
        private readonly IStorageService _storage;

        public DownloadFileQueryHandler(IStorageService storage) => _storage = storage;

        public Task<(string Filename, byte[] Content)> Handle(DownloadFileQuery req, CancellationToken ct) =>
            _storage.DownloadAsync(req.Id);
    }

}
