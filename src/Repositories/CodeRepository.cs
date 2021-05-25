using System.Threading.Tasks;

namespace DownloadService
{
    public interface ICodeRepository
    {
        Task<Code> GetCode(string code);
    }

    public class CodeRepository : ICodeRepository
    {
        public async Task<Code> GetCode(string code)
        {
            return null;
        }
    }
}