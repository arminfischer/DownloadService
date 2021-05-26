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
        public ReleaseRepository(IOptions<DownloadServiceConfiguration> options) : base(GetTableName(), options) { }

        public async Task<Release> GetRelease(string releaseName)
        {
            Release r = await client.GetEntityAsync<Release>(GetPartitionKey(), releaseName);
            return r;
        }

        private static string GetTableName()
        {
            return "Releases";
        }

        private string GetPartitionKey()
        {
            return "default"; // This can later be changed to the user
        }
    }
}