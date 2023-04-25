using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Export
{
    public interface IExportService
    {
        public Task Export(string content);
    }
}
