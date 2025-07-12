namespace StorageService.Application.IServices
{
    public interface IStorageService
    {
        Task<string> UploadAsync(string filename, byte[] content);
        Task<bool> DeleteAsync(string id);
        Task<(string Filename, byte[] Content)> DownloadAsync(string id);
    }
}
