using StorageService.Application.IServices;


namespace StorageService.Infrastructure.Antivirus
{
    public class FileScannerService : IFileScannerService
    {
        public Task<bool> IsFileInfected(byte[] content) => Task.FromResult(false);
    }
}
