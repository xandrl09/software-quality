namespace Stocks.Services.Files
{
    public interface IFileService
    {
        public Task SaveContent(string content);
        public Task<string> LoadContent(DateTime date);
        public Task<string> LoadLastAvailableContent();
        public string GetLastAvailableFilePath();
    }
}
