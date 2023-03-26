using CsvHelper;
using CsvHelper.Configuration;
using Stocks.Services.Models;
using System.Globalization;

namespace Stocks.Services.Parsers;

public class CsvParser : IParser
{
    public async Task<IEnumerable<StockModel>> GetStocksAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            AllowComments = true,
            Comment = '"'
        };
        using var csv = new CsvReader(reader, configuration);
        csv.Context.RegisterClassMap<StockModelMap>();
        var records = csv.GetRecordsAsync<StockModel>();
        return await records.ToListAsync();
    }
}