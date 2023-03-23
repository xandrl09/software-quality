namespace Stocks.HttpClientArk;

public class Download
{
    private HttpClient _client;

    public Download(HttpClient client)
    {
        _client = client;
    }
    public async Task<string?> GetEtfHoldingsCsv(string path)
    {
        string? csv = null;
        HttpResponseMessage responseMessage = await _client.GetAsync(path);
        if (responseMessage.IsSuccessStatusCode)
        {
            csv = await responseMessage.Content.ReadAsStringAsync();
        }
        return csv;
    }

}