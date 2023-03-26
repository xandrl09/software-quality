using Stocks.Services.Diff;
using Stocks.Services.Files;
using Stocks.Services.Helpers;
using Stocks.Services.HttpClientArk;
using Stocks.Services.Models;
using Stocks.Services.Parsers;

namespace Stocks.Console;

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
        System.Console.WriteLine(csv);
        var fileService = new DateFileService(FILE_DIRECTORY, FILE_FORMAT, FILE_EXTENSION);
        await fileService.SaveContent(csv);

        var loadedCsv = await fileService.LoadLastAvailableContent();

        string pathToRecentFile = PathHelper.GetDateFilePath(DateTime.Today, FILE_FORMAT, FILE_DIRECTORY, FILE_EXTENSION);
        string pathToOlderFile = fileService.GetLastAvailableFilePath();

        var parser = new CsvParser();
        var recentHoldings = await parser.GetStocksAsync(pathToRecentFile);
        var pastHoldings = await parser.GetStocksAsync(pathToOlderFile);

        var diffService = new HoldingsDifferenceService();
        var diffResult = diffService.GetDifference(recentHoldings, pastHoldings);
        
        PrintResultToConsole(diffResult);
    }

    private void PrintResultToConsole(HoldingsDifferenceModel diffResult)
    {
        System.Console.WriteLine("New positions:");
        foreach(var newPositon in diffResult.NewPositions)
        {
            System.Console.WriteLine($"{newPositon.Ticker}, {newPositon.Company}, {newPositon.Shares}, {newPositon.Weight}");
        }

        System.Console.WriteLine("Increased positions:");
        foreach (var increasedPositon in diffResult.IncreasedPositons)
        {
            System.Console.WriteLine($"{increasedPositon.Ticker}, {increasedPositon.CompanyName}, {increasedPositon.DifferenceInShares}({increasedPositon.PercentageDifferenceInShares}%), {increasedPositon.Weight}");
        }

        System.Console.WriteLine("Reduced positions:");
        foreach (var reducedPosition in diffResult.ReducedPositions)
        {
            System.Console.WriteLine($"{reducedPosition.Ticker}, {reducedPosition.CompanyName}, {reducedPosition.DifferenceInShares}({reducedPosition.PercentageDifferenceInShares}%), {reducedPosition.Weight}");
        }
    }

    public void RunClient()
    {
        RunAsync().GetAwaiter().GetResult();
    }
}