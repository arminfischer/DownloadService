using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public interface IFileRepository
    {
        Task<Uri> GetTemporaryUri(string filename);
    }

    public class FileRepository : IFileRepository
    {
        private DownloadServiceConfiguration downloadServiceConfiguration;

        public FileRepository(IOptions<DownloadServiceConfiguration> options)
        {
            this.downloadServiceConfiguration = options.Value;
        }

        public async Task<Uri> GetTemporaryUri(string filename)
        {
            BlobClient client = new BlobClient(downloadServiceConfiguration.AzureStorageConnectionString, downloadServiceConfiguration.ContainerNameFiles, filename);
            return client.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(1));
        }
    }
}