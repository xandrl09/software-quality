using Stocks.Services.Models;

namespace Stocks.Services.Parsers
{
    /// <summary>
    /// Interface <c>IParseService</c> represents a service for parsing files.
    /// </summary>
    public interface IParseService
    {
        /// <summary>
        /// Parses a file and returns a list of stocks.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A list of <c>StockModel</c> objects.</returns>
        public Task<IEnumerable<StockModel>> GetStocksAsync(string filePath);
    }
}