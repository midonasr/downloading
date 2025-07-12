using System;

namespace StorageService.Domain.Entities
{
    public class FileMetadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Filename { get; set; }
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public bool IsInfected { get; set; }
    }
}
