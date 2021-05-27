namespace DownloadService
{
    public class DownloadServiceConfiguration
    {
        public string AzureStorageConnectionString { get; set; }
        public string ContainerNameFiles { get; set; }
        public string TableNameCodes { get; set; }
        public string TableNameReleases { get; set; }
    }
}