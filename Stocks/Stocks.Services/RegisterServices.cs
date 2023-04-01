using Microsoft.Extensions.DependencyInjection;
using Stocks.Services.Diff;
using Stocks.Services.Files;
using Stocks.Services.HttpClientArk;
using Stocks.Services.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public static class RegisterServices
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<Download>();
            services.AddTransient<IHoldingsDifferenceService, HoldingsDifferenceService>();
            services.AddTransient<IFileService, DateFileService>();
            services.AddTransient<IParser, CsvParser>();
        }
    }
}
