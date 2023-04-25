using Microsoft.Extensions.Configuration;
using Stocks.Services.Helpers;
using Stocks.Services.Models.Configuration;
using System.Globalization;
using Stocks.Services.Exceptions;

namespace Stocks.Services.Files;

public class DateFileService : IFileService
{
    private readonly Settings _settings;

    public DateFileService(Settings settings)
    {
        _settings = settings;
    }

    public async Task SaveContent(string content, string extension)
    {
        string path = GetPathByDate(DateTime.Today, extension);
        await File.WriteAllTextAsync(path, content);
    }

    public async Task<string> LoadContent(DateTime date, string extension)
    {
        string path = GetPathByDate(date, extension);
        string content = await LoadContent(path);
        return content;
    }

    public async Task<string> LoadLastAvailableContent(string dir, string extension)
    {
        return await LoadContent(GetLastAvailableFilePath(dir, extension));
    }

    public string GetLastAvailableFilePath(string dir, string extension)
    {
        var availableFileNames = Directory.GetFiles(dir, $"*{extension}");

        var lastKnownFile = availableFileNames
            .Select(x => new KeyValuePair<DateTime, string>(ParseFileName(x), x))
            .OrderByDescending(x => x.Key)
            .Skip(1)
            .FirstOrDefault()
            .Value;

        if (lastKnownFile == null)
        {
            throw new CsvFilePathNotFoundException();
        }

        return lastKnownFile;
    }

    private string GetPathByDate(DateTime date, string extension)
    {
        return PathHelper.GetDateFilePath(date, _settings.FileNameFormat, _settings.SaveDirectory,
            extension);
    }

    private async Task<string> LoadContent(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    private DateTime ParseFileName(string fileName)
    {
        return DateTime.ParseExact(s: Path.GetFileNameWithoutExtension(fileName), format: _settings.FileNameFormat,
            provider: CultureInfo.CurrentCulture);
    }
}