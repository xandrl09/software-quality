namespace Stocks.Services.Download
{
    /// <summary>
    /// Interface <c>IDownloadService</c> defines the contract for the service that downloads a file.
    /// </summary>
    public interface IDownloadService
    {
        /// <summary>
        /// Downloads a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The file content.</returns>
        public Task<string?> DownloadFile(string path);
    }
}