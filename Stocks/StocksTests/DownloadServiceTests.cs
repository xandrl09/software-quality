using Stocks.Services.Models;
using Stocks.Services.Exceptions;
using FakeItEasy;
namespace Stocks.Services.Client;

public class DownloadServiceTests
{
    private DownloadService _downloadService;

    [Test]
    public void DownloadService_NoConnection_ThrowsCustomException()
    {
        // arrange
        

        // act

        // assert
        
    }

    [Test]
    public void DownloadService_NotDownloadedStocks_ThrowsCustomException()
    {
        // arrange


        // act

        // assert

    }
}