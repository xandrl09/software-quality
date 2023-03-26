using Stocks.Files;
using Stocks.Helpers;
using Stocks.Parsers;

namespace Stocks.HttpClientArk;

public class Client
{
    static readonly HttpClient _client = new();
    private const string FILE_DIRECTORY = ".";
    private const string FILE_FORMAT = "dd_MM_yyyy";
    private const string FILE_EXTENSION = ".csv";

    public Client()
    {
        
    }
    private async Task RunAsync()
    {
        var download = new Download(_client);
        var urlPath = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var csv = await download.GetEtfHoldingsCsv(urlPath);
        if (string.IsNullOrEmpty(csv))
        {
            return;
        }
        Console.WriteLine(csv);
        var fileService = new DateFileService(FILE_DIRECTORY, FILE_FORMAT, FILE_EXTENSION);
        await fileService.SaveContent(csv);

        var loadedCsv = await fileService.LoadLastAvailableContent();
        var parser = new CsvParser();

        string path = PathHelper.GetDateFilePath(DateTime.Today, FILE_FORMAT, FILE_DIRECTORY, FILE_EXTENSION);

        var stocks = await parser.GetStocksAsync(path);
    }

    public void RunClient()
    {
        RunAsync().GetAwaiter().GetResult();
    }
}