using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DownloadService.Test
{
    public class DownloadServiceTest
    {
        private IDownloadService downloadService;
        private ILogger logger;
        private IReleaseRepository releaseRepository;
        private ICodeRepository codeRepository;
        private IFileRepository fileRepository;

        [SetUp]
        public void Setup()
        {
            releaseRepository = Substitute.For<IReleaseRepository>();
            codeRepository  = Substitute.For<ICodeRepository>();
            fileRepository = Substitute.For<IFileRepository>();
            downloadService = new DownloadService(releaseRepository, codeRepository, fileRepository);

            logger = Substitute.For<ILogger>();
        }

        [Test]
        public void DownloadFile_Should_ReturnTemporaryFileUrl_When_ReleaseAndCodeExists()
        {
            // Arrange
            releaseRepository.GetRelease("myrelease").Returns(Task.FromResult(new Release { Url = "http://myrelease" }));
            codeRepository.GetCode("code123").Returns(Task.FromResult(new Code()));
            fileRepository.GetTemporaryUrl("http://myrelease").Returns(Task.FromResult("http://mydownloadfile"));
            DownloadFile downloadFile = new DownloadFile(downloadService);

            // Act
            RedirectResult result = (RedirectResult)downloadFile.Run(Substitute.For<HttpRequest>(), "myrelease", "code123", logger).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result.Url, "http://mydownloadfile");
        }
    }
}