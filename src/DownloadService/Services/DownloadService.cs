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
            Release release = await releaseRepository.Get(releaseName);
            Code code = await codeRepository.Get(releaseName, codeValue);

            if (!code.FurtherDownloadsAllowed(release))
            {
                throw new ArgumentException($"The code is not enabled for further downloads, maximum downloads of {release.MaxAllowedDownloads} reached.");
            }

            string temporaryUrl = (await fileRepository.GetTemporaryUri(release.Url)).AbsoluteUri;

            code.Downloads++;

            await codeRepository.Update(code).ConfigureAwait(false);

            return temporaryUrl;
        }
    }
}