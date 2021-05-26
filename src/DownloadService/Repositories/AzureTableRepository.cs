using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DownloadService
{
    public class AzureTableRepository
    {
        private readonly DownloadServiceConfiguration configuration;

        public AzureTableRepository(IOptions<DownloadServiceConfiguration> options)
        {
            configuration = options.Value;
        }

        protected T GetEntity<T>(string table, string partitionKey, string rowKey)
        {
            return default; // TableClient client = new TableClient()
        }
    }
}