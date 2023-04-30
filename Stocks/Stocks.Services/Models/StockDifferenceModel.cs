namespace Stocks.Services.Models
{
    /// <summary>
    /// Class <c>StockDifferenceModel</c> represents the difference of a stock between two dates.
    /// </summary>
    public class StockDifferenceModel
    {
        public string CompanyName { get; set; }
        public string Ticker { get; set; }
        public int DifferenceInShares { get; set; }
        public double PercentageDifferenceInShares { get; set; }
        public string Weight { get; set; }
        public string Cusip { get; set; }
    }
}