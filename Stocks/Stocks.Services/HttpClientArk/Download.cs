using Microsoft.Extensions.Configuration;
using Stocks.Services.Models.Configuration;
using System.IO;

namespace Stocks.Services.HttpClientArk;

public class Download
{
    private HttpClient _client;
    private readonly Settings _settings;

    public Download(IConfiguration configuration)
    {
        _settings = Settings.Get(configuration);
        _client = new HttpClient();
    }

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

    public async Task<string?> GetEtfHoldingsCsv(string path)
    {
        string? csv = null;

        var requestMessage = CreateGetRequestMessage(path);
        HttpResponseMessage responseMessage = await _client.SendAsync(requestMessage);
        
        if (responseMessage.IsSuccessStatusCode)
        {
            csv = await responseMessage.Content.ReadAsStringAsync();
        }
        return csv;
    }

}