 
using StorageService.Infrastructure.Storage;
using StorageService.Infrastructure.Antivirus;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Application.IRepository;
using StorageService.Application.IServices;
using StorageService.Infrastructure.Repository;

namespace StorageService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection s)
        {
            s.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
            s.AddScoped<IStorageService, MinioStorageService>();
            s.AddScoped<IFileScannerService, FileScannerService>();
            return s;
        }
    }
}
