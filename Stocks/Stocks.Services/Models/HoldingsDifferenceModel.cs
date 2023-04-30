namespace Stocks.Services.Models
{
    /// <summary>
    /// Class <c>HoldingsDifferenceModel</c> represents the difference of holdings between two dates.
    /// </summary>
    public class HoldingsDifferenceModel
    {
        public IEnumerable<StockModel> NewPositions { get; set; }
        public IEnumerable<StockDifferenceModel> IncreasedPositons { get; set; }
        public IEnumerable<StockDifferenceModel> ReducedPositions { get; set; }
    }
}