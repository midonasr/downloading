using StorageService.Application.IRepository;
using StorageService.Domain.Entities;
using StorageService.Infrastructure.Persistence.StorageService.Infrastructure.Persistence;

namespace StorageService.Infrastructure.Repository
{
    public class FileMetadataRepository : IFileMetadataRepository
    {
        private readonly StorageDbContext _db;
        public FileMetadataRepository(StorageDbContext db) => _db = db;
        public async Task SaveAsync(FileMetadata m)
        {
            _db.Files.Add(m);
            await _db.SaveChangesAsync();
        }
        public async Task<FileMetadata> GetByIdAsync(string id)
        {
            return await _db.Files.FindAsync(id)
                   ?? throw new KeyNotFoundException($"File '{id}' not found");
        }
        public async Task DeleteByIdAsync(string id)
        {
            var entity = await _db.Files.FindAsync(id);
            if (entity == null) return;
            _db.Files.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
