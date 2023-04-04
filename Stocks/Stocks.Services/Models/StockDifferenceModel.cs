using System;

namespace Stocks.Services.Models
{
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