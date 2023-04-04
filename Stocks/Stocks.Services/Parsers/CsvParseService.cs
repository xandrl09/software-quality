using CsvHelper;
using CsvHelper.Configuration;
using Stocks.Services.Models;
using System.Globalization;

namespace Stocks.Services.Parsers;

public class CsvParseService : IParseService
{
    public async Task<IEnumerable<StockModel>> GetStocksAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);

        // Solution to exception when reading blank line taken from https://stackoverflow.com/a/57994196
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            AllowComments = true,
            Comment = '"',
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            ShouldSkipRecord = args => args.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
        };
        using var csv = new CsvReader(reader, configuration);
        csv.Context.RegisterClassMap<StockModelMap>();
        var records = csv.GetRecordsAsync<StockModel>();
        return await records.ToListAsync();
    }
}