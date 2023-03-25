namespace Stocks.Files;

public class DateFileService: IFileService
{
    private string _fileNameFormat;
    private string _saveDirectory;
    private string _extension;

    public DateFileService(string saveDirectory,
        string fileNameFormat,
        string extension)
    {
        _saveDirectory = saveDirectory;
        _fileNameFormat = fileNameFormat;
        _extension = extension;
     }

    private string GetFilePath(DateTime date)
    {
        string path = string.Format(_fileNameFormat, date);
        path = Path.Join(_saveDirectory, path + _extension);
        return path;
    }

    public async Task SaveContent(string content)
    {
        string path = GetFilePath(DateTime.Today);
        await File.WriteAllTextAsync(path, content);
    }

    public async Task<string> LoadContent(DateTime date)
    {
        string path = GetFilePath(date);
        string content = await File.ReadAllTextAsync(path);
        return content;
    }
}