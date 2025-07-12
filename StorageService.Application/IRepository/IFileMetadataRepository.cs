using StorageService.Domain.Entities;

namespace StorageService.Application.IRepository
{
    public interface IFileMetadataRepository
    {
        Task SaveAsync(FileMetadata metadata);
        Task<FileMetadata> GetByIdAsync(string id);
        Task DeleteByIdAsync(string id);
    }
}
