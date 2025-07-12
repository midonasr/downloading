namespace StorageService.Application.IServices
{
    public interface IFileScannerService
    {
        Task<bool> IsFileInfected(byte[] content);
    }
}
