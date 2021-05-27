using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DownloadService.Startup))]

namespace DownloadService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddLogging()
                .AddSingleton<IDownloadService, DownloadService>()
                .AddSingleton<IReleaseRepository, ReleaseRepository>()
                .AddSingleton<ICodeRepository, CodeRepository>()
                .AddSingleton<IFileRepository, FileRepository>()
                .AddOptions<DownloadServiceConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });
        }
    }
}