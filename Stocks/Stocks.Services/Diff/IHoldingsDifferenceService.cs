using Stocks.Services.Models;

namespace Stocks.Services.Diff
{
    /// <summary>
    /// Interface <c>IHoldingsDifferenceService</c> defines the contract for the service that calculates the difference in positions between two sets of holdings.
    /// </summary>
    public interface IHoldingsDifferenceService
    {
        /// <summary>
        /// Calculates the difference in positions between two sets of holdings.
        /// </summary>
        /// <param name="actualHoldings">The actual holdings.</param>
        /// <param name="pastHoldings">The past holdings.</param>
        /// <returns>The difference in positions between two sets of holdings.</returns>
        public HoldingsDifferenceModel GetDifference(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockModel> pastHoldings);
    }
}