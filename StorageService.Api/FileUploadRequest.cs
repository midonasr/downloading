namespace StorageService.Api
{
    public class FileUploadRequest
    {
        public string UploaderName { get; set; }
        public IFormFile File { get; set; }
    }
}
