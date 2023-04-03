namespace Stocks.Services.Client
{
    public interface IDownloadService
    {
        public Task<string?> DownloadFile(string path);
    }
}
