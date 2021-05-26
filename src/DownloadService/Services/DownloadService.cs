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
            if (!IsValidRelease(release))
            {
                throw new ArgumentException("The release is not valid.");
            }

            Code code = await codeRepository.GetCode(releaseName, codeValue);
            if (!IsValidCode(release, code))
            {
                throw new ArgumentException("The code is not valid.");
            }

            string temporaryUrl = await fileRepository.GetTemporaryUrl(release.Url);
            
            code.Downloads = code.Downloads + 1;
            //codeRepository.Update(code);

            return temporaryUrl;
        }

        private bool IsValidRelease(Release release)
        {
            return release != null;
        }

        private bool IsValidCode(Release release, Code code)
        {
            if (code == null || release.MaxAllowedDownloads > 0 && code.Downloads > release.MaxAllowedDownloads)
            {
                return false;
            }

            return true;
        }
    }
}