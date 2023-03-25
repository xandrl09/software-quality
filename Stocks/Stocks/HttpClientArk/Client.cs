using Stocks.Files;
using Stocks.Parsers;

namespace Stocks.HttpClientArk;

public class Client
{
    static readonly HttpClient _client = new();
    private const string FILE_DIRECTORY = ".";
    private const string FILE_FORMAT = "{0:dd_MM_yyyy}";

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
        var fileService = new DateFileService(FILE_DIRECTORY, FILE_FORMAT, ".csv");
        await fileService.SaveContent(csv);

        var yesterday = DateTime.Now.AddDays(-1);
        var loadedCsv = await fileService.LoadContent(yesterday);
        var parser = new CsvParser();

        string path = string.Format(FILE_FORMAT, DateTime.Today);
        path = Path.Join(FILE_DIRECTORY, path + ".csv");

        var stocks = await parser.GetStocksAsync(path);
    }

    public void RunClient()
    {
        RunAsync().GetAwaiter().GetResult();
    }
}