using Microsoft.Extensions.Configuration;
using Stocks.Services.Diff;
using Stocks.Services.Files;
using Stocks.Services.Helpers;
using Stocks.Services.Download;
using Stocks.Services.Exceptions;
using Stocks.Services.Export;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Parsers;
using Stocks.Services.Output;

namespace Stocks.Console;

/// <summary>
/// Class <c>Client</c> is the entry point of the application.
/// </summary>
public class Client
{
    private readonly IDownloadService _downloadService;
    private readonly IFileService _dateFileService;
    private readonly IParseService _parserService;
    private readonly IHoldingsDifferenceService _differenceService;
    private readonly IOutputService _outputService;
    private readonly IExportService _exportService;
    private readonly Settings _settings;

    public Client(IDownloadService downloadService,
        IFileService dateFileService,
        IParseService parserService,
        IHoldingsDifferenceService differenceService,
        Settings settings,
        IOutputService outputService,
        IExportService exportService)
    {
        _downloadService = downloadService;
        _dateFileService = dateFileService;
        _parserService = parserService;
        _differenceService = differenceService;
        _outputService = outputService;
        _exportService = exportService;
        _settings = settings;
    }

    /// <summary>
    /// Runs the application.
    /// </summary>
    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Runs the application asynchronously.
    /// In case of an exception, the exception message is printed to the console.
    /// </summary>
    public async Task RunAsync()
    {
        string? csv;
        try
        {
            csv = await _downloadService.DownloadFile(_settings.CsvUrl);

            if (string.IsNullOrEmpty(csv))
            {
                System.Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.EmptyCsvFile));
                return;
            }

            await _dateFileService.SaveContent(csv, _settings.FileExtension);

            string pathToRecentFile = PathHelper.GetDateFilePath(DateTime.Today, _settings.FileNameFormat,
                _settings.SaveDirectory, _settings.FileExtension);
            string pathToOlderFile;

            pathToOlderFile =
                _dateFileService.GetLastAvailableFilePath(_settings.SaveDirectory, _settings.FileExtension);


            IEnumerable<StockModel> recentHoldings;
            IEnumerable<StockModel> pastHoldings;

            recentHoldings = await _parserService.GetStocksAsync(pathToRecentFile);
            pastHoldings = await _parserService.GetStocksAsync(pathToOlderFile);


            var diffResult = _differenceService.GetDifference(recentHoldings, pastHoldings);

            PrintResultToConsole(diffResult);

            var htmlOutput = await _outputService.GenerateOutput(diffResult);

            await _dateFileService.SaveContent(htmlOutput, ".html");

            await _exportService.Export(htmlOutput);
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
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return;
        }
    }

    /// <summary>
    /// Prints the result to the console.
    /// </summary>
    /// <param name="diffResult">Difference in positions of the holdings.</param>
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