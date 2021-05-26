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
        public CodeRepository(IOptions<DownloadServiceConfiguration> options) : base(GetTableName(), options) { }

        public async Task<Code> GetCode(string releaseName, string code)
        {
            return await client.GetEntityAsync<Code>(releaseName, code);
        }

        private static string GetTableName()
        {
            return "Codes";
        }
    }
}