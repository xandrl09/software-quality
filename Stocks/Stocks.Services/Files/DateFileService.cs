using Microsoft.Extensions.Configuration;
using Stocks.Services.Helpers;
using Stocks.Services.Models.Configuration;
using System.Globalization;
using Stocks.Services.Exceptions;

namespace Stocks.Services.Files;

public class DateFileService : IFileService
{
    private readonly Settings _settings;

    public DateFileService(IConfiguration configuration)
    {
        _settings = Settings.Get(configuration);
    }

    public async Task SaveContent(string content)
    {
        string path = GetPathByDate(DateTime.Today);
        await File.WriteAllTextAsync(path, content);
    }

    public async Task<string> LoadContent(DateTime date)
    {
        string path = GetPathByDate(date);
        string content = await LoadContent(path);
        return content;
    }

    public async Task<string> LoadLastAvailableContent()
    {
        return await LoadContent(GetLastAvailableFilePath());
    }

    public string GetLastAvailableFilePath()
    {
        var availableFileNames = Directory.GetFiles(_settings.SaveDirectory, $"*{_settings.FileExtension}");

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

    private string GetPathByDate(DateTime date)
    {
        return PathHelper.GetDateFilePath(date, _settings.FileNameFormat, _settings.SaveDirectory,
            _settings.FileExtension);
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