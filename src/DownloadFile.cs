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
        private readonly IDownloadService _downloadService;

        public DownloadFile(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        [FunctionName("DownloadFile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "download/{releaseName}/{codeValue}")] HttpRequest req, string releaseName, string codeValue,
            ILogger log)
        {
            try
            {
                string url = await _downloadService.GetFileUrl(releaseName, codeValue);

                return new RedirectResult(url);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}