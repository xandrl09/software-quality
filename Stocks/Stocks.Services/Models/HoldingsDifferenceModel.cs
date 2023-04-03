namespace Stocks.Services.Models
{
    public class HoldingsDifferenceModel
    {
        public IEnumerable<StockModel> NewPositions { get; set; }
        public IEnumerable<StockDifferenceModel> IncreasedPositons { get; set; }
        public IEnumerable<StockDifferenceModel> ReducedPositions { get; set; }
    }
}