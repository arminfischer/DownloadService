using System;
using System.Threading.Tasks;

namespace DownloadService
{
    public interface IDownloadService
    {
        Task<string> GetFileUrl(string releaseName, string codeValue);
    }

    public class DownloadService : IDownloadService
    {
        private readonly ICodeRepository codeRepository;
        private readonly IFileRepository fileRepository;
        private readonly IReleaseRepository releaseRepository;

        public DownloadService(IReleaseRepository releaseRepository, ICodeRepository codeRepository, IFileRepository fileRepository)
        {
            this.releaseRepository = releaseRepository;
            this.codeRepository = codeRepository;
            this.fileRepository = fileRepository;
        }

        public async Task<string> GetFileUrl(string releaseName, string codeValue)
        {
            Release release = await releaseRepository.GetRelease(releaseName);

            if (release != null)
            {
                Code code = await codeRepository.GetCode(codeValue);
                string temporaryUrl = fileRepository.GetTemporaryUrl(code.Url);
                code.Downloads
                codeRepository.Update(code);
                
                return temporaryUrl; 
            }
            else
            {
                throw new ArgumentException("The release is not valid.");
            }
        }
    }
}