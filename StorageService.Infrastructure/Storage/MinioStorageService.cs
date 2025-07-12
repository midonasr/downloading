
using Amazon.S3;
using Amazon.S3.Transfer; 
using StorageService.Application.IServices;

namespace StorageService.Infrastructure.Storage
{
    public class MinioStorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "files";

        public MinioStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
        }

        public async Task<string> UploadAsync(string filename, byte[] content)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename is required", nameof(filename));
            if (content == null || content.Length == 0)
                throw new ArgumentException("File content is required", nameof(content));

            var id = Guid.NewGuid().ToString();

            using var stream = new MemoryStream(content);
            var request = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = id,
                BucketName = BucketName,
                ContentType = "application/octet-stream"
            };
            var transfer = new TransferUtility(_s3Client);
            await transfer.UploadAsync(request).ConfigureAwait(false);

            return id;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("File ID is required", nameof(id));

            var response = await _s3Client.DeleteObjectAsync(BucketName, id).ConfigureAwait(false);
            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent ||
                   response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<(string Filename, byte[] Content)> DownloadAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("File ID is required", nameof(id));

            var response = await _s3Client.GetObjectAsync(BucketName, id).ConfigureAwait(false);
            using var stream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(stream).ConfigureAwait(false);

            return (response.Key, stream.ToArray());
        }
    }
}
