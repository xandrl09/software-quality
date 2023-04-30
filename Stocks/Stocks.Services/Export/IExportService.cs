using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Export
{
    /// <summary>
    /// Interface <c>IExportService</c> defines the contract for the service that exports content.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exports the content.
        /// </summary>
        /// <param name="content">The content to be exported.</param>
        /// <returns>Task.</returns>
        public Task Export(string content);
    }
}