using Stocks.Services.Models;

namespace Stocks.Services.Parsers
{
    public interface IParseService
    {
        public Task<IEnumerable<StockModel>> GetStocksAsync(string filePath);
    }
}