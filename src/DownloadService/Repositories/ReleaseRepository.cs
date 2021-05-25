using System.Threading.Tasks;

namespace DownloadService
{
    public interface IReleaseRepository
    {
        Task<Release> GetRelease(string release);
    }

    public class ReleaseRepository : IReleaseRepository
    {
        public async Task<Release> GetRelease(string release)
        {
            return null;
        }
    }
}