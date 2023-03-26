using Stocks.Services.Helpers;
using System.Globalization;

namespace Stocks.Services.Files;

public class DateFileService : IFileService
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

    private string GetPathByDate(DateTime date)
    {
        return PathHelper.GetDateFilePath(date, _fileNameFormat, _saveDirectory, _extension);
    }

    public async Task SaveContent(string content)
    {
        string path = GetPathByDate(DateTime.Today);
        await File.WriteAllTextAsync(path, content);
    }

    private async Task<string> LoadContent(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    public async Task<string> LoadContent(DateTime date)
    {
        string path = GetPathByDate(date);
        string content = await LoadContent(path);
        return content;
    }

    private DateTime ParseFileName(string fileName)
    {
        return DateTime.ParseExact(s: Path.GetFileNameWithoutExtension(fileName), format: _fileNameFormat, provider: CultureInfo.CurrentCulture);
    }

    public async Task<string> LoadLastAvailableContent()
    {
        return await LoadContent(GetLastAvailableFilePath());
    }

    public string GetLastAvailableFilePath()
    {
        var availableFileNames = Directory.GetFiles(_saveDirectory, $"*{_extension}");

        return availableFileNames
            .Select(x => new KeyValuePair<DateTime, string>(ParseFileName(x), x))
            .OrderByDescending(x => x.Key)
            .Skip(1)
            .FirstOrDefault()
            .Value;
    }
}