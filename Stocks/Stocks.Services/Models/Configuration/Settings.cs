using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Models.Configuration
{
    public class Settings
    {
        public string SaveDirectory { get; set; }
        public string FileNameFormat { get; set; }
        public string FileExtension { get; set; }
        public string CsvUrl { get; set; }
        public string UserAgent { get; set; }

        public static Settings Get(IConfiguration configuration)
        {
            return configuration.GetRequiredSection("Settings").Get<Settings>();
        }
    }
}
