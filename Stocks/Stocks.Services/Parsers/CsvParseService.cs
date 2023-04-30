using CsvHelper;
using CsvHelper.Configuration;
using Stocks.Services.Models;
using System.Globalization;

namespace Stocks.Services.Parsers;

/// <summary>
/// Class <c>CsvParseService</c> represents a service for parsing CSV files.
/// </summary>
public class CsvParseService : IParseService
{
    /// <summary>
    /// Parses a CSV file.
    /// </summary>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>A list of <c>StockModel</c> objects.</returns>
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