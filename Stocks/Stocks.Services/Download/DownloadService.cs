using Microsoft.Extensions.Configuration;
using Stocks.Services.Exceptions;
using Stocks.Services.Models.Configuration;

namespace Stocks.Services.Download;

/// <summary>
/// Class <c>DownloadService</c> is responsible for downloading files.
/// </summary>
public class DownloadService : IDownloadService
{
    private readonly HttpClient _client;
    private readonly Settings _settings;

    /// <summary>
    /// Initializes a new instance of <see cref="DownloadService"/>.
    /// </summary>
    /// <param name="settings">Settings.</param>
    /// <param name="client">HTTP client.</param>
    public DownloadService(Settings settings, 
        HttpClient client)
    {
        _settings = settings;
        _client = client;
    }

    /// <summary>
    /// Downloads a file asynchronously.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>The file content.</returns>
    /// <exception cref="InvalidDownloadException">Thrown when the download fails.</exception>
    public async Task<string?> DownloadFile(string path)
    {
        string? csv = null;
        var requestMessage = CreateGetRequestMessage(path);
       
        HttpResponseMessage responseMessage = await _client.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new InvalidDownloadException(ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload));
        }

        csv = await responseMessage.Content.ReadAsStringAsync();

        return csv;
    }

    /// <summary>
    /// Creates a GET request message.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns>The request message.</returns>
    private HttpRequestMessage CreateGetRequestMessage(string url)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("User-Agent", _settings.UserAgent);
        return request;
    }
}