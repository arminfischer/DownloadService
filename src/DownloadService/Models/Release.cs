namespace DownloadService {
    public class Release {
        public string Url { get; set; }
        public int MaxAllowedDownloads { get; internal set; }
    }
}