using Microsoft.Extensions.Configuration;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Output
{
    public class FileOutputService : IOutputService
    {
        private readonly Settings _settings;

        public FileOutputService(IConfiguration configuration)
        {
            _settings = Settings.Get(configuration);
        }

        /// <summary>
        /// Outputs results to file
        /// </summary>
        /// <param name="differences">Results to output</param>
        /// <param name="destination">Path to output results</param>
        public Task Output(HoldingsDifferenceModel differences, string destination)
        {
            return Task.CompletedTask;
        }
    }
}
