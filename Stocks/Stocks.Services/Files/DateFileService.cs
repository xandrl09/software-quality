using Microsoft.Extensions.Configuration;
using Stocks.Services.Helpers;
using Stocks.Services.Models.Configuration;
using System.Globalization;
using Stocks.Services.Exceptions;

namespace Stocks.Services.Files;

/// <summary>
/// Class <c>DateFileService</c> is responsible for saving and loading files by date.
/// </summary>
public class DateFileService : IFileService
{
    private readonly Settings _settings;

    /// <summary>
    /// Initializes a new instance of <see cref="DateFileService"/>.
    /// </summary>
    /// <param name="settings"><c>Settings</c>.</param>
    public DateFileService(Settings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Saves the content to a file named by today's date asynchronously.
    /// </summary>
    /// <param name="content">Content to save.</param>
    /// <param name="extension">File extension.</param>
    public async Task SaveContent(string content, string extension)
    {
        string path = GetPathByDate(DateTime.Today, extension);
        await File.WriteAllTextAsync(path, content);
    }

    /// <summary>
    /// Loads the content from a file named by the given date asynchronously.
    /// </summary>
    /// <param name="date">Date for which to load the content.</param>
    /// <param name="extension">File extension.</param>
    /// <returns>The file content.</returns>
    public async Task<string> LoadContent(DateTime date, string extension)
    {
        string path = GetPathByDate(date, extension);
        string content = await LoadContent(path);
        return content;
    }

    /// <summary>
    /// Loads the content from a last available file asynchronously.
    /// </summary>
    /// <param name="dir">Directory to search for the file.</param>
    /// <param name="extension">File extension.</param>
    /// <returns>Path of the last available file.</returns>
    /// <exception cref="CsvFilePathNotFoundException">Thrown when the file is not found.</exception>
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

    /// <summary>
    /// Gets the path by date and extension from the directory specified in the settings.
    /// </summary>
    /// <param name="date">Date for which to get the path.</param>
    /// <param name="extension">File extension.</param>
    /// <returns>The path.</returns>
    private string GetPathByDate(DateTime date, string extension)
    {
        return PathHelper.GetDateFilePath(date, _settings.FileNameFormat, _settings.SaveDirectory,
            extension);
    }

    /// <summary>
    /// Loads the content from the given path asynchronously.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <returns>The file content.</returns>
    private async Task<string> LoadContent(string path)
    {
        return await File.ReadAllTextAsync(path);
    }

    /// <summary>
    /// Parses the file name to a date time according to the given format in the settings.
    /// </summary>
    /// <param name="fileName">File name to parse.</param>
    /// <returns>The date time.</returns>
    private DateTime ParseFileName(string fileName)
    {
        return DateTime.ParseExact(s: Path.GetFileNameWithoutExtension(fileName), format: _settings.FileNameFormat,
            provider: CultureInfo.CurrentCulture);
    }
}