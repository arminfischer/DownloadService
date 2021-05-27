namespace DownloadService
{
    public class DownloadServiceConfiguration
    {
        public string AzureWebJobsStorage { get; set; }
        public string ContainerNameFiles { get; set; }
        public string TableNameCodes { get; set; }
        public string TableNameReleases { get; set; }
    }
}