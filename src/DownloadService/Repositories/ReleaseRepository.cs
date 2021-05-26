using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public interface IReleaseRepository
    {
        Task<Release> GetRelease(string releaseName);
    }

    public class ReleaseRepository : AzureTableRepository, IReleaseRepository
    {
        public ReleaseRepository(IOptions<DownloadServiceConfiguration> options) : base(options) { }

        public async Task<Release> GetRelease(string releaseName)
        {
            return GetEntity<Release>(GetTableName(), GetPartitionKey(), releaseName);
        }

        private string GetTableName()
        {
            return "Releases";
        }

        private string GetPartitionKey()
        {
            return "default"; // This can later be changed to the user
        }
    }
}