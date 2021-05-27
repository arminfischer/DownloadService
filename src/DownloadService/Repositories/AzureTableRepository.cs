using Azure.Data.Tables;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public class AzureTableRepository
    {
        internal TableClient TableClient { get; private set; }

        public AzureTableRepository(string table, IOptions<DownloadServiceConfiguration> options)
        {
            TableClient = new TableClient(options.Value.AzureWebJobsStorage, table);
        }
    }
}