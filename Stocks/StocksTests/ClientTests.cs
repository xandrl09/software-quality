using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Stocks.Console;
using Stocks.Services.Client;
using Stocks.Services.Diff;
using Stocks.Services.Exceptions;
using Stocks.Services.Files;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Output;
using Stocks.Services.Parsers;


namespace StocksTests;

public class ClientTests
{
    private  IDownloadService _download;
    private  IFileService _dateFileService;
    private  IParseService _parser;
    private  IHoldingsDifferenceService _differenceService;
    private  IOutputService _outputService;
    private  IConfiguration _configuration;
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
        _settings.CsvUrl = "invalid_path";
        _settings.SaveDirectory= ".";
        _settings.FileExtension=".csv";
        _settings.FileNameFormat = "dd_MM_yyyy";
        _settings.UserAgent = "StockService/1.0";
    }
    
    [Test]
    public async Task Client_RunAsync_DownloadServiceThrowsException()
    {
        
        A.CallTo(() => _download.DownloadFile(_settings.CsvUrl)).Throws<InvalidDownloadException>();
        var client = new Client(_download, _dateFileService, _parser, _differenceService, _configuration, _outputService);
        Assert.ThrowsAsync<InvalidDownloadException>(async () => await client.RunAsync());
        // await client.RunAsync();
        
    }
}