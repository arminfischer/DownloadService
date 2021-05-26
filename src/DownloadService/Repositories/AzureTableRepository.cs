using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DownloadService
{
    public class AzureTableRepository
    {
        internal TableClient client { get; private set; }

        public AzureTableRepository(string table, IOptions<DownloadServiceConfiguration> options)
        {
            client = new TableClient(options.Value.AzureStorageConnectionString, table);
        }
    }
}