using Stocks.HttpClientArk;

namespace Stocks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.RunClient();
        }
    }
}