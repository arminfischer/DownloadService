using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public interface IReleaseRepository
    {
        Task<Release> Get(string releaseName);
    }

    public class ReleaseRepository : AzureTableRepository, IReleaseRepository
    {
        public ReleaseRepository(IOptions<DownloadServiceConfiguration> options) : base(options.Value.TableNameReleases, options) { }

        public async Task<Release> Get(string releaseName)
        {
            try
            {
                return await TableClient.GetEntityAsync<Release>(GetPartitionKey(), releaseName);
            }
            catch (Azure.RequestFailedException exception)
            {
                throw new ArgumentException($"Release could not be retrieved ({exception.ErrorCode}).");
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message);
            }
        }

        private string GetPartitionKey()
        {
            return "default"; // This can later be changed to the user
        }
    }
}