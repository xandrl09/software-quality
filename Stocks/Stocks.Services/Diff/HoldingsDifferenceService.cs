using Stocks.Services.Models;

namespace Stocks.Services.Diff
{
    public class HoldingsDifferenceService : IHoldingsDifferenceService
    {
        private const int NO_DIFFERENCE_IN_SHARES = 0;

        public HoldingsDifferenceModel GetDifference(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockModel> pastHoldings)
        {
            var result = new HoldingsDifferenceModel();
            var overlappingPositions = GetOverlappingPositions(actualHoldings, pastHoldings);
            result.NewPositions = GetNewPositions(actualHoldings, overlappingPositions);
            result.IncreasedPositons = GetIncreasedPositions(overlappingPositions);
            result.ReducedPositions = GetReducedPositions(overlappingPositions).Concat(GetReducedToZeroPositions(pastHoldings, overlappingPositions));

            return result;
        }

        private IEnumerable<StockModel> GetNewPositions(IEnumerable<StockModel> actualHoldings,
            IEnumerable<StockDifferenceModel> overlap)
        {
            return actualHoldings.Where(actual => overlap.All(past => past.Ticker != actual.Ticker));
        }

        private IEnumerable<StockDifferenceModel> GetIncreasedPositions(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares > NO_DIFFERENCE_IN_SHARES);
        }

        private IEnumerable<StockDifferenceModel> GetReducedPositions(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares < NO_DIFFERENCE_IN_SHARES);
        }

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

        private double CalculatePercentageDifference(double newValue, double oldValue)
        {
            const int HUNDRED_PERCENT = 100;
            const int NUMBER_OF_DECIMAL_PLACES = 2;

            var percentageDifference = ((newValue - oldValue) / oldValue) * HUNDRED_PERCENT;

            return Math.Round(percentageDifference, NUMBER_OF_DECIMAL_PLACES);
        }
    }
}