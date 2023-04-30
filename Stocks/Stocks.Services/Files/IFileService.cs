namespace Stocks.Services.Files
{
    /// <summary>
    /// Interface <c>IFileService</c> defines the contract for the service that handles files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Saves the content.
        /// </summary>
        /// <param name="content">Content to be saved.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <returns>Task.</returns>
        public Task SaveContent(string content, string extension);

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="date">Date from which the content should be loaded.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <returns>The content asynchronously.</returns>
        public Task<string> LoadContent(DateTime date, string extension);

        /// <summary>
        /// Gets the path of the last available file.
        /// </summary>
        /// <param name="dir">Directory to search for the file.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <returns>The path of the last available file.</returns>
        public string GetLastAvailableFilePath(string dir, string extension);
    }
}