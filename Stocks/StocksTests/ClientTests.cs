using Stocks.Services.Exceptions;


namespace StocksTests;

public class ClientTests
{
    
    [Test]
    public async Task Client_RunAsync_DownloadServiceThrowsException()
    {
        var client = new MockedClient();
        client
            .CanNotDownloadData()
            .RunAsync()
            .AssertException(ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload));
    }
    
    [Test]
    public async Task Client_RunAsync_DownloadsEmptyFile()
    {
        var client = new MockedClient();
        client
            .DownloadEmptyData()
            .RunAsync()
            .AssertException(ExceptionStrings.GetExceptionMessage(CustomException.EmptyCsvFile));
    }
    [Test]
    public async Task Client_RunAsync_DateFileServiceThrowsException()
    {
        var client = new MockedClient();
        client
            .CanDownloadData()
            .CanSaveContent()
            .CanNotGetLastAvailableFilePath()
            .RunAsync()
            .AssertException(ExceptionStrings.GetExceptionMessage(CustomException.CsvFilePathNotFound));
    }

    [Test]
    public async Task Client_RunAsync_DateFileService_SaveContentThrowsException()
    {
        var client = new MockedClient();
        client
            .CanDownloadData()
            .CanNotSaveContent()
            .RunAsync()
            .AssertException(ExceptionStrings.GetExceptionMessage(CustomException.IoException));
    }

    [Test]
    public async Task Client_RunAsync_ParserThrowsMissingFieldException()
    {
        var client = new MockedClient();
        client
            .CanDownloadData()
            .CanSaveContent()
            .CanGetLastAvailableFilePath()
            .CanNotParseData()
            .RunAsync()
            .AssertException(ExceptionStrings.GetExceptionMessage(CustomException.MissingFieldException));
    }

    [Test]
    public async Task Client_RunAsync_SuccesfullyOutputsToConsole()
    {
        var client = new MockedClient();
        client
            .CanDownloadData()
            .CanSaveContent()
            .CanGetLastAvailableFilePath()
            .CanParseData()
            .CanGetDifference()
            .RunAsync()
            .AssertContainsStockInfo("Microsoft");
    }
}