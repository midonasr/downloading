namespace StorageService.Api
{
    public class MinioSettings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceURL { get; set; }
        public bool UseHTTPS { get; set; } = false;
        public bool ForcePathStyle { get; set; } = true;
    }
}
