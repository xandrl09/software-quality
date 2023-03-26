using System.IO;

namespace Stocks.Services.HttpClientArk;

public class Download
{
    private HttpClient _client;
    private const string USER_AGENT = "StocksService/1.0";

    public Download(HttpClient client)
    {
        _client = client;
    }

    private HttpRequestMessage CreateGetRequestMessage(string url)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(url),
            Method = HttpMethod.Get
        };
        request.Headers.Add("User-Agent", USER_AGENT);
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