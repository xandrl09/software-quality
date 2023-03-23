namespace Stocks.HttpClientArk;

public class Client
{
    static readonly HttpClient _client = new();

    public Client()
    {
        
    }
    private async Task RunAsync()
    {
        var download = new Download(_client);
        var urlPath = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var csv = await download.GetEtfHoldingsCsv(urlPath);
        if (csv is not null)
        {
            Console.WriteLine(csv);
        }
    }

    public void RunClient()
    {
        RunAsync().GetAwaiter().GetResult();
    }
}