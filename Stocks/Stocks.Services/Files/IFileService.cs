namespace Stocks.Services.Files
{
    public interface IFileService
    {
        public Task SaveContent(string content, string extension);
        public Task<string> LoadContent(DateTime date, string extension);
        public Task<string> LoadLastAvailableContent(string dir, string extension);
        public string GetLastAvailableFilePath(string dir, string extension);
    }
}