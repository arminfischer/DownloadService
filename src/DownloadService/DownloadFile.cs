using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace DownloadService
{
    public class DownloadFile
    {
        private readonly IDownloadService downloadService;
        private readonly ILogger<DownloadFile> logger;

        public DownloadFile(IDownloadService downloadService, ILogger<DownloadFile> logger)
        {
            this.downloadService = downloadService;
            this.logger = logger;
        }

        [FunctionName("DownloadFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download/{releaseName}/{codeValue}")] HttpRequest req, string releaseName, string codeValue)
        {
            try
            {
                string url = await downloadService.GetFileUrl(releaseName, codeValue);

                return new RedirectResult(url);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}