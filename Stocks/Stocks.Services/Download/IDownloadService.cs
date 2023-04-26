namespace Stocks.Services.Download
{
    public interface IDownloadService
    {
        public Task<string?> DownloadFile(string path);
    }
}