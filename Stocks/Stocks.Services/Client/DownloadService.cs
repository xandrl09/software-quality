﻿using Microsoft.Extensions.Configuration;
using Stocks.Services.Models.Configuration;

namespace Stocks.Services.Client;

public class DownloadService : IDownloadService
{
    private readonly HttpClient _client;
    private readonly Settings _settings;

    public DownloadService(IConfiguration configuration)
    {
        _settings = Settings.Get(configuration);
        _client = new HttpClient();
    }

    public async Task<string?> DownloadFile(string path)
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