using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Stocks.Console;
using Stocks.Services.Download;
using Stocks.Services.Diff;
using Stocks.Services.Exceptions;
using Stocks.Services.Export;
using Stocks.Services.Files;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Output;
using Stocks.Services.Parsers;

namespace StocksTests;

public class MockedClient
{
    public Client TestClient { get; }
    private StringWriter? ConsoleOutput { get; set; }
    private IDownloadService _downloadService;
    private IFileService _dateFileService;
    private IParseService _parserService;
    private IHoldingsDifferenceService _differenceService;
    private IOutputService _outputService;
    private IExportService _exportService;
    private Settings _settings;

    public void SetUp()
    {
        _downloadService = A.Fake<IDownloadService>();
        _dateFileService = A.Fake<IFileService>();
        _parserService = A.Fake<IParseService>();
        _differenceService = A.Fake<IHoldingsDifferenceService>();
        _outputService = A.Fake<IOutputService>();
        _exportService = A.Fake<IExportService>();

        _settings = new Settings();
        _settings.CsvUrl =
            "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        _settings.SaveDirectory = ".";
        _settings.FileExtension = ".csv";
        _settings.FileNameFormat = "dd_MM_yyyy";
        _settings.UserAgent = "StockService/1.0";
        
        ConsoleOutput = new StringWriter();
        Console.SetOut(ConsoleOutput);
    }

    public MockedClient()
    {
        SetUp();
        TestClient = new Client(_downloadService, 
            _dateFileService, 
            _parserService, 
            _differenceService, 
            _settings,
            _outputService, 
            _exportService);
    }

    public MockedClient CanDownloadData()
    {
        A.CallTo(() => _downloadService.DownloadFile(_settings.CsvUrl)).Returns(" ");
        return this;
    }

    public MockedClient CanNotDownloadData()
    {
        A.CallTo(() => _downloadService.DownloadFile(_settings.CsvUrl)).Throws<InvalidDownloadException>();
        return this;
    }
    
    public MockedClient DownloadEmptyData()
    {
        A.CallTo(() => _downloadService.DownloadFile(_settings.CsvUrl)).Returns("");
        return this;
    }
    
    public MockedClient CanSaveContent()
    {
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored, _settings.FileExtension)).DoesNothing();
        return this;
    }
    public MockedClient CanNotSaveContent()
    {
        A.CallTo(() => _dateFileService.SaveContent(A<string>.Ignored, _settings.FileExtension)).Throws<IOException>();
        return this;
    }
    
    public MockedClient CanGetLastAvailableFilePath()
    {
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath(_settings.SaveDirectory, _settings.FileExtension)).Returns("valid path");
        return this;
    }
    
    public MockedClient CanNotGetLastAvailableFilePath()
    {
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath(_settings.SaveDirectory, _settings.FileExtension)).Throws<CsvFilePathNotFoundException>();
        return this;
    }
    
    public MockedClient CanNotParseData()
    {
        A.CallTo(() => _parserService.GetStocksAsync(A<string>.Ignored)).Throws<MissingFieldException>();
        return this;
    }
    
    public MockedClient CanParseData()
    {
        var stocksList = new List<StockModel>();
        var stockModel = new StockModel
            { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "15%" };
        stocksList.Add(stockModel);

        HoldingsDifferenceModel model = new HoldingsDifferenceModel();
        model.NewPositions = stocksList;
        model.IncreasedPositons = new List<StockDifferenceModel>();
        model.ReducedPositions = new List<StockDifferenceModel>();
        
        A.CallTo(() => _parserService.GetStocksAsync(A<string>.Ignored)).Returns(stocksList);
        return this;
    }
    
    public MockedClient GetLastAvailableFilePathThrowsException()
    {
        A.CallTo(() => _dateFileService.GetLastAvailableFilePath(_settings.SaveDirectory, _settings.FileExtension)).Throws<CsvFilePathNotFoundException>();
        return this;
    }
    
    public  MockedClient RunAsync()
    {
        TestClient.Run();
        return this;
    }
    
    public  MockedClient CanGetDifference()
    {
        var stocksList = new List<StockModel>();
        var stockModel = new StockModel
            { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "15%" };
        stocksList.Add(stockModel);
        
        HoldingsDifferenceModel model = new HoldingsDifferenceModel();
        model.NewPositions = stocksList;
        model.IncreasedPositons = new List<StockDifferenceModel>();
        model.ReducedPositions = new List<StockDifferenceModel>();
        
        A.CallTo(() =>
                _differenceService.GetDifference(A<IEnumerable<StockModel>>.Ignored,
                    A<IEnumerable<StockModel>>.Ignored))
            .Returns(model);
        return this;
    }
    
    public void AssertException(string assertError)
    {
        Assert.That(assertError,
            Is.EqualTo(GetConsoleOutput()));
    }

    public string GetConsoleOutput()
    {
        return ConsoleOutput.ToString().Trim();
    }

    public void AssertContainsStockInfo(string expected)
    {
        StringAssert.Contains(expected, GetConsoleOutput());
    }

}