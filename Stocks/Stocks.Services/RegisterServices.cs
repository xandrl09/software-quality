using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Services.Download;
using Stocks.Services.Diff;
using Stocks.Services.Export;
using Stocks.Services.Files;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Output;
using Stocks.Services.Parsers;

namespace Stocks.Services
{
    /// <summary>
    /// Registers services for dependency injection.
    /// </summary>
    public static class RegisterServices
    {
        public static void AddStocksServices(this IServiceCollection services)
        {
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IHoldingsDifferenceService, HoldingsDifferenceService>();
            services.AddTransient<IFileService, DateFileService>();
            services.AddTransient<IParseService, CsvParseService>();
            services.AddTransient<IOutputService, HtmlOutputService>();
            services.AddTransient<IExportService, EmailExportService>();
            services.AddSingleton<Settings>(f =>
            {
                var configuration = f.GetRequiredService<IConfiguration>();
                return Settings.Get(configuration);
            });
        }
    }
}