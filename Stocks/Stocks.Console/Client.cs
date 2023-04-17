using Microsoft.Extensions.Configuration;
using Stocks.Services.Diff;
using Stocks.Services.Files;
using Stocks.Services.Helpers;
using Stocks.Services.Client;
using Stocks.Services.Exceptions;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Parsers;
using Stocks.Services.Output;

namespace Stocks.Console;

public class Client
{
    private readonly IDownloadService _download;
    private readonly IFileService _dateFileService;
    private readonly IParseService _parser;
    private readonly IHoldingsDifferenceService _differenceService;
    private readonly IOutputService _outputService;
    private readonly Settings _settings;

    public Client(IDownloadService download,
        IFileService dateFileService,
        IParseService parser,
        IHoldingsDifferenceService differenceService,
        IConfiguration configuration,
        IOutputService outputService)
    {
        _download = download;
        _dateFileService = dateFileService;
        _parser = parser;
        _differenceService = differenceService;
        _outputService = outputService;

        _settings = Settings.Get(configuration);
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public async Task RunAsync()
    {
        string? csv;
        try {

            csv = await _download.DownloadFile(_settings.CsvUrl);

            if (string.IsNullOrEmpty(csv))
            {
                System.Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.EmptyCsvFile));
                return;
            }

            await _dateFileService.SaveContent(csv);
         
            string pathToRecentFile = PathHelper.GetDateFilePath(DateTime.Today, _settings.FileNameFormat,
                _settings.SaveDirectory, _settings.FileExtension);
            string pathToOlderFile;
       
            pathToOlderFile = _dateFileService.GetLastAvailableFilePath();
       

            IEnumerable<StockModel> recentHoldings;
            IEnumerable<StockModel> pastHoldings;
      
            recentHoldings = await _parser.GetStocksAsync(pathToRecentFile);
            pastHoldings = await _parser.GetStocksAsync(pathToOlderFile);
        

            var diffResult = _differenceService.GetDifference(recentHoldings, pastHoldings);

            PrintResultToConsole(diffResult);

            await _outputService.Output(diffResult, _settings.SaveDirectory);
            
        }
        catch (CsvFilePathNotFoundException e)
        {
            System.Console.WriteLine(e.Message);
            return;
        }
        catch (IOException)
        {
            System.Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.IoException));
            return;
        }
        catch (InvalidDownloadException e)
        {
            System.Console.WriteLine(e.Message);
            return;
        }
        catch (MissingFieldException)
        {
            System.Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.MissingFieldException));
            return;
        }
        catch (Exception e) {
            System.Console.WriteLine(e.Message);
            return;
        }
       
    }

    private void PrintResultToConsole(HoldingsDifferenceModel diffResult)
    {
        System.Console.WriteLine("New positions:");
        foreach (var newPositon in diffResult.NewPositions)
        {
            System.Console.WriteLine(
                $"{newPositon.Ticker}, {newPositon.Company}, {newPositon.Shares}, {newPositon.Weight}");
        }

        System.Console.WriteLine("Increased positions:");
        foreach (var increasedPositon in diffResult.IncreasedPositons)
        {
            System.Console.WriteLine(
                $"{increasedPositon.Ticker}, {increasedPositon.CompanyName}, {increasedPositon.DifferenceInShares}({increasedPositon.PercentageDifferenceInShares}%), {increasedPositon.Weight}");
        }

        System.Console.WriteLine("Reduced positions:");
        foreach (var reducedPosition in diffResult.ReducedPositions)
        {
            System.Console.WriteLine(
                $"{reducedPosition.Ticker}, {reducedPosition.CompanyName}, {reducedPosition.DifferenceInShares}({reducedPosition.PercentageDifferenceInShares}%), {reducedPosition.Weight}");
        }
    }
}