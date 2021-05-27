using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DownloadService
{
    public interface ICodeRepository
    {
        Task<Code> Get(string releaseName, string code);
        Task Update(Code code);
    }

    public class CodeRepository : AzureTableRepository, ICodeRepository
    {
        public CodeRepository(IOptions<DownloadServiceConfiguration> options) : base(options.Value.TableNameCodes, options) { }

        public async Task<Code> Get(string releaseName, string code)
        {
            try
            {
                return await TableClient.GetEntityAsync<Code>(releaseName, code);
            }
            catch (Azure.RequestFailedException exception)
            {
                throw new ArgumentException($"Code could not be retrieved ({exception.ErrorCode}).");
            }
            catch (Exception exception)
            {
                throw new ArgumentException(exception.Message);
            }
        }

        public async Task Update(Code code)
        {
            await TableClient.UpdateEntityAsync<Code>(code, code.ETag);
        }
    }
}