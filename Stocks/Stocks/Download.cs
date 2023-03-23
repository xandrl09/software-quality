namespace Stocks;

public class Download
{
    private static HttpClient _client = new HttpClient();

    static async Task<string?> GetEtfHoldingsCsv(string path)
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