using System;
using Azure;
using Azure.Data.Tables;

namespace DownloadService {
    public class Code : ITableEntity
    {
        public int Downloads { get; set; }
        public string PartitionKey { get ; set; }
        public string RowKey { get ; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get ; set; }

        public bool FurtherDownloadsAllowed(Release release)
        {
            if (release.MaxAllowedDownloads > 0 && this.Downloads >= release.MaxAllowedDownloads)
            {
                return false;
            }

            return true;
        }
    }
}