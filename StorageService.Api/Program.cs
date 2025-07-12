using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using StorageService.Infrastructure.Extensions;
using StorageService.Api.Services; 
using StorageService.Infrastructure.Persistence.StorageService.Infrastructure.Persistence;
using StorageService.Api;  
using System.Reflection;
using Amazon.S3.Model;
using Minio;
using Minio.DataModel.Args;

var builder = WebApplication.CreateBuilder(args);

// Load configs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddControllers();

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(5001, o => o.Protocols = HttpProtocols.Http2);
});

// AppSettings binding



builder.Services.AddAWSService<IAmazonS3>();

var minio = builder.Configuration.GetSection("Minio").Get<MinioSettings>();

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var cfg = new AmazonS3Config
    {
        ServiceURL = minio.ServiceURL,
        ForcePathStyle = minio.ForcePathStyle,
        UseHttp = !minio.UseHTTPS
    };
    return new AmazonS3Client(minio.AccessKey, minio.SecretKey, cfg);
});

 


 
static async Task EnsureBucketExistsAsync(IAmazonS3 client, string bucketName)
{
    var minio = new MinioClient()
    .WithEndpoint("localhost", 9000)
    .WithCredentials("minioadmin", "minioadmin123")
    .WithSSL(false)
    .Build();

    bool found = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket("files"));
    if (!found)
        await minio.MakeBucketAsync(new MakeBucketArgs().WithBucket("files"));
}

// DB
builder.Services.AddDbContext<StorageDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Infrastructure registration
builder.Services.AddInfrastructureServices();
 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());                  // API handlers
    cfg.RegisterServicesFromAssembly(typeof(StorageService.Application.Commands.UploadFileCommand).Assembly);  // Application handlers
});
// Map middlewares and endpoints
var app = builder.Build();
// After configuring IAmazonS3
var s3Client = app.Services.GetRequiredService<IAmazonS3>();
await EnsureBucketExistsAsync(s3Client, "files");

app.UseSwagger();
app.UseSwaggerUI();
app.MapGrpcService<StorageGrpcService>();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
    db.Database.Migrate();
}
app.Run();
