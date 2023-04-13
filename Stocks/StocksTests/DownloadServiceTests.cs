using Stocks.Services.Models;
using Stocks.Services.Exceptions;
using FakeItEasy;
namespace Stocks.Services.Client;

public class DownloadServiceTests
{
    private IDownloadService _downloadService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _downloadService = A.Fake<IDownloadService>();
    }

    [Test]
    public async Task DownloadService_NotDownloaded_ThrowsCustomException()
    {
        A.CallTo(() => _downloadService.DownloadFile("invalid_path")).Throws<InvalidTimeZoneException>();
        
    }
}