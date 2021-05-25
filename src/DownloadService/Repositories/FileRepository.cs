using System.Threading.Tasks;

namespace DownloadService
{
    public interface IFileRepository
    {
        Task<string> GetTemporaryUrl(string url);
    }

    public class FileRepository : IFileRepository
    {
        public async Task<string> GetTemporaryUrl(string url)
        {
            return null;
        }
    }
}