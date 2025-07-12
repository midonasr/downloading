 
namespace StorageService.Infrastructure.Persistence
{
    using global::StorageService.Domain.Entities;
    using Microsoft.EntityFrameworkCore; 
 

    namespace StorageService.Infrastructure.Persistence
    {
        public class StorageDbContext : DbContext
        {
            public StorageDbContext(DbContextOptions<StorageDbContext> opts) : base(opts) { }
            public DbSet<FileMetadata> Files { get; set; }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.Entity<FileMetadata>().HasKey(e => e.Id);
            }
        }
    }

}
