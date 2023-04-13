namespace Stocks.Services.Models
{
    public class StockModel
    {
        public string Company { get; init; }
        public string Ticker { get; init; }
        public int Shares { get; init; }
        public string Weight { get; init; }
        public string Cusip { get; init; }
    }
}