using System;
using Azure;
using Azure.Data.Tables;

namespace DownloadService
{
    public class Release : ITableEntity
    {
        public string Url { get; set; }
        public int MaxAllowedDownloads { get; set; }
        public string PartitionKey { get ; set; }
        public string RowKey { get ; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get ; set; }
    }
}