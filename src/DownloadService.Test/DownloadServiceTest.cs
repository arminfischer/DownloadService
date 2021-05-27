using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace DownloadService.Test
{
    public class DownloadServiceTest
    {
        private IDownloadService downloadService;
        private ILogger<DownloadFile> logger;
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

            logger = Substitute.For<ILogger<DownloadFile>>();
        }

        [Test]
        public void DownloadFile_Should_ReturnTemporaryFileUrl_When_ReleaseAndCodeExists()
        {
            // Arrange
            releaseRepository.Get("myrelease").Returns(Task.FromResult(new Release { Url = "http://myrelease" }));
            codeRepository.Get("myrelease", "code123").Returns(Task.FromResult(new Code()));
            fileRepository.GetTemporaryUri("http://myrelease").Returns(Task.FromResult(new Uri("http://mydownloadfile")));
            DownloadFile downloadFile = new DownloadFile(downloadService, logger);

            // Act
            RedirectResult result = (RedirectResult)downloadFile.Run(Substitute.For<HttpRequest>(), "myrelease", "code123").GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual("http://mydownloadfile/", result.Url);
        }

        [Test]
        public void DownloadFile_Should_ReturnBadRequest_When_ReleaseDoesNotExist()
        {
            // Arrange
            releaseRepository.Get("myrelease").Returns(Task.FromException<Release>(new Exception("some release error")));
            DownloadFile downloadFile = new DownloadFile(downloadService, logger);

            // Act
            BadRequestObjectResult result = (BadRequestObjectResult)downloadFile.Run(Substitute.For<HttpRequest>(), "myrelease", "code123").GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual("There was an error downloading the file: some release error", result.Value);
        }

        [Test]
        public void DownloadFile_Should_ReturnBadRequest_When_CodeDoesNotExist()
        {
            // Arrange
            releaseRepository.Get("myrelease").Returns(Task.FromResult(new Release { Url = "http://myrelease" }));
            codeRepository.Get("myrelease", "code123").Returns(Task.FromException<Code>(new Exception("some code error")));
            DownloadFile downloadFile = new DownloadFile(downloadService, logger);

            // Act
            BadRequestObjectResult result = (BadRequestObjectResult)downloadFile.Run(Substitute.For<HttpRequest>(), "myrelease", "code123").GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual("There was an error downloading the file: some code error", result.Value);
        }

        [Test]
        public void DownloadFile_Should_ReturnBadRequest_When_CodeHasBeenUsedTooOften()
        {
            // Arrange
            releaseRepository.Get("myrelease").Returns(Task.FromResult(new Release { Url = "http://myrelease", MaxAllowedDownloads = 2 }));
            codeRepository.Get("myrelease", "code123").Returns(Task.FromResult(new Code { Downloads = 2 }));
            DownloadFile downloadFile = new DownloadFile(downloadService, logger);

            // Act
            BadRequestObjectResult result = (BadRequestObjectResult)downloadFile.Run(Substitute.For<HttpRequest>(), "myrelease", "code123").GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual("There was an error downloading the file: The code is not enabled for further downloads, maximum downloads of 2 reached.", result.Value);
        }
    }
}