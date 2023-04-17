using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Stocks.Console;
using Stocks.Services.Client;
using Stocks.Services.Diff;
using Stocks.Services.Exceptions;
using Stocks.Services.Files;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Output;
using Stocks.Services.Parsers;


namespace StocksTests;

public class ClientTests
{
    private IDownloadService _download;
    private IFileService _dateFileService;
    private IParseService _parser;
    private IHoldingsDifferenceService _differenceService;
    private IOutputService _outputService;
    private IConfiguration _configuration;
    private Settings _settings;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _download = A.Fake<IDownloadService>();
        _dateFileService = A.Fake<IFileService>();
        _parser = A.Fake<IParseService>();
        _differenceService = A.Fake<IHoldingsDifferenceService>();
        _outputService = A.Fake<IOutputService>();
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _settings = new Settings();
        _settings.CsvUrl =
            "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        _settings.SaveDirectory = ".";
        _settings.FileExtension = ".csv";
        _settings.FileNameFormat = "dd_MM_yyyy";
        _settings.UserAgent = "StockService/1.0";
    }

    [Test]
    public async Task Client_RunAsync_DownloadServiceThrowsException()
    {
        // arrange
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Throws<InvalidDownloadException>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        Assert.That(ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload),
            Is.EqualTo(consoleOutput.ToString().Trim()));
    }

    [Test]
    public async Task Client_RunAsync_DownloadsEmptyFile()
    {
        // arrange
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Returns("");
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        Assert.That(ExceptionStrings.GetExceptionMessage(CustomException.EmptyCsvFile),
            Is.EqualTo(consoleOutput.ToString().Trim()));
    }


    [Test]
    public async Task Client_RunAsync_DateFileServiceThrowsException()
    {
        // arrange
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Returns(" ");
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored)).DoesNothing();
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath()).Throws<CsvFilePathNotFoundException>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        Assert.That(ExceptionStrings.GetExceptionMessage(CustomException.CsvFilePathNotFound),
            Is.EqualTo(consoleOutput.ToString().Trim()));
    }

    [Test]
    public async Task Client_RunAsync_DateFileService_SaveContentThrowsException()
    {
        // arrange
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Returns("valid csv");
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored)).Throws<IOException>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        Assert.That(ExceptionStrings.GetExceptionMessage(CustomException.IoException),
            Is.EqualTo(consoleOutput.ToString().Trim()));
    }

    [Test]
    public async Task Client_RunAsync_ParserThrowsMissingFieldException()
    {
        // arrange
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Returns("valid csv");
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored)).DoesNothing();
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath()).Returns("valid path");
        A.CallTo(() => _parser.GetStocksAsync(A<string>.Ignored)).Throws<MissingFieldException>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        Assert.That(ExceptionStrings.GetExceptionMessage(CustomException.MissingFieldException),
            Is.EqualTo(consoleOutput.ToString().Trim()));
    }

    [Test]
    public async Task Client_RunAsync_SuccesfullyOutputsToConsole()
    {
        // arrange
        var stocksList = new List<StockModel>();
        var stockModel = new StockModel
            { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "15%" };
        stocksList.Add(stockModel);

        HoldingsDifferenceModel model = new HoldingsDifferenceModel();
        model.NewPositions = stocksList;
        model.IncreasedPositons = new List<StockDifferenceModel>();
        model.ReducedPositions = new List<StockDifferenceModel>();

        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Returns("valid csv");
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored)).DoesNothing();
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath()).Returns("valid path");
        A.CallTo(() => _parser.GetStocksAsync(A<string>.Ignored)).Returns(stocksList);
        A.CallTo(() =>
                _differenceService.GetDifference(A<IEnumerable<StockModel>>.Ignored,
                    A<IEnumerable<StockModel>>.Ignored))
            .Returns(model);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration,
            _outputService);

        // act
        await client.RunAsync();

        // assert
        Assert.IsNotEmpty(consoleOutput.ToString());
        StringAssert.Contains("Microsoft", consoleOutput.ToString().Trim());
    }
}