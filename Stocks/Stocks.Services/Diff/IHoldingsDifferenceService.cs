using Stocks.Services.Models;

namespace Stocks.Services.Diff
{
    public interface IHoldingsDifferenceService
    {
        public HoldingsDifferenceModel GetDifference(IEnumerable<StockModel> actualHoldings, IEnumerable<StockModel> pastHoldings);
    }
}