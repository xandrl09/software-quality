using Stocks.Services.Models;

namespace Stocks.Services.Diff
{
    /// <summary>
    /// Class <c>HoldingsDifferenceService</c> calculates the difference in positions between two sets of holdings.
    /// </summary>
    public class HoldingsDifferenceService : IHoldingsDifferenceService
    {
        private const int NO_DIFFERENCE_IN_SHARES = 0;

        /// <summary>
        /// Calculates the difference in positions between two sets of holdings.
        /// </summary>
        /// <param name="actualHoldings">Today's holdings.</param>
        /// <param name="pastHoldings">Holdings from a previous date.</param>
        /// <returns><c>HoldingsDifferenceModel</c> object containing the difference in positions between two sets of holdings.</returns>
        public HoldingsDifferenceModel GetDifference(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockModel> pastHoldings)
        {
            var result = new HoldingsDifferenceModel();
            var overlappingPositions = GetOverlappingPositions(actualHoldings, pastHoldings);
            result.NewPositions = GetNewPositions(actualHoldings, overlappingPositions);
            result.IncreasedPositons = GetIncreasedPositions(overlappingPositions);
            result.ReducedPositions = GetReducedPositions(overlappingPositions)
                .Concat(GetReducedToZeroPositions(pastHoldings, overlappingPositions));

            return result;
        }

        /// <summary>
        /// Gets holdings that are new.
        /// </summary>
        /// <param name="actualHoldings"></param>
        /// <param name="overlap">The holdings that overlap.</param>
        /// <returns>The holdings that are new.</returns>
        private IEnumerable<StockModel> GetNewPositions(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockDifferenceModel> overlap)
        {
            return actualHoldings.Where(actual => overlap.All(past => past.Ticker != actual.Ticker));
        }

        /// <summary>
        /// Gets holdings that have increased their position.
        /// </summary>
        /// <param name="overlap">The holdings that overlap.</param>
        /// <returns>The holdings that have increased their position.</returns>
        private IEnumerable<StockDifferenceModel> GetIncreasedPositions(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares > NO_DIFFERENCE_IN_SHARES);
        }

        /// <summary>
        /// Gets holdings that have reduced their position.
        /// </summary>
        /// <param name="overlap">The holdings that overlap.</param>
        /// <returns>The holdings that have reduced their position.</returns>
        private IEnumerable<StockDifferenceModel> GetReducedPositions(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares < NO_DIFFERENCE_IN_SHARES);
        }

        /// <summary>
        /// Gets holdings that have reduced their position to zero.
        /// </summary>
        /// <param name="pastHoldings">Holdings from a previous date.</param>
        /// <param name="overlap">The holdings that overlap.</param>
        /// <returns>The holdings that have reduced their position to zero.</returns>
        private IEnumerable<StockDifferenceModel> GetReducedToZeroPositions(IEnumerable<StockModel> pastHoldings,
            IEnumerable<StockDifferenceModel> overlap)
        {
            const int MAXIMAL_NEGATIVE_PERCENTAGE_DIFFERENCE = -100;
            const string MINIMAL_PERCENTAGE_WEIGHT = "0%";

            return pastHoldings
                .Where(past => overlap.All(actual => actual.Cusip != past.Cusip))
                .Select(past => new StockDifferenceModel
                {
                    Ticker = past.Ticker,
                    CompanyName = past.Company,
                    DifferenceInShares = -past.Shares,
                    PercentageDifferenceInShares = MAXIMAL_NEGATIVE_PERCENTAGE_DIFFERENCE,
                    Weight = MINIMAL_PERCENTAGE_WEIGHT,
                    Cusip = past.Cusip,
                });
        }

        /// <summary>
        /// Gets holdings that overlap between two sets of holdings.
        /// </summary>
        /// <param name="actualHoldings">Today's holdings.</param>
        /// <param name="pastHoldings">Holdings from a previous date.</param>
        /// <returns>The holdings that overlap between two sets of holdings.</returns>
        private IEnumerable<StockDifferenceModel> GetOverlappingPositions(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockModel> pastHoldings)
        {
            return pastHoldings.Join(
                actualHoldings,
                past => past.Cusip,
                actual => actual.Cusip,
                (past, actual) => new StockDifferenceModel
                {
                    Ticker = actual.Ticker,
                    CompanyName = actual.Company,
                    DifferenceInShares = actual.Shares - past.Shares,
                    PercentageDifferenceInShares = CalculatePercentageDifference(actual.Shares, past.Shares),
                    Weight = actual.Weight,
                    Cusip = actual.Cusip,
                });
        }

        /// <summary>
        /// Calculates the percentage difference between two values.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <returns>The percentage difference between two values.</returns>
        private double CalculatePercentageDifference(double newValue, double oldValue)
        {
            const int HUNDRED_PERCENT = 100;
            const int NUMBER_OF_DECIMAL_PLACES = 2;

            var percentageDifference = ((newValue - oldValue) / oldValue) * HUNDRED_PERCENT;

            return Math.Round(percentageDifference, NUMBER_OF_DECIMAL_PLACES);
        }
    }
}