using System;
using Azure;
using Azure.Data.Tables;

namespace DownloadService {
    public class Code : ITableEntity
    {
        public int Downloads { get; internal set; }
        public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DateTimeOffset? Timestamp => throw new NotImplementedException();

        public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}