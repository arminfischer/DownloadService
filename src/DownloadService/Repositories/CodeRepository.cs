using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public interface ICodeRepository
    {
        Task<Code> GetCode(string releaseName, string code);
    }

    public class CodeRepository : AzureTableRepository, ICodeRepository
    {
        public CodeRepository(IOptions<DownloadServiceConfiguration> options) : base(options) { }

        public async Task<Code> GetCode(string releaseName, string code)
        {
            return GetEntity<Code>(GetTableName(), releaseName, code);
        }

        private string GetTableName()
        {
            return "Codes";
        }
    }
}