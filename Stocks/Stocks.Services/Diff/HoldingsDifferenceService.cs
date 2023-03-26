using Stocks.Services.Models;

namespace Stocks.Services.Diff
{
    public class HoldingsDifferenceService : IHoldingsDifferenceService
    {
        public HoldingsDifferenceModel GetDifference(IEnumerable<StockModel> actualHoldings, IEnumerable<StockModel> pastHoldings)
        {
            var result = new HoldingsDifferenceModel();
            var overlappingPositions = GetOverlappingPositions(actualHoldings, pastHoldings);
            result.NewPositions = GetNewPositons(actualHoldings, overlappingPositions);
            result.IncreasedPositons = GetIncreasedPositons(overlappingPositions);
            result.ReducedPositions = GetReducedPositions(overlappingPositions).Concat(GetReducedToZeroPositions(pastHoldings, overlappingPositions));

            return result;
        }

        private IEnumerable<StockModel> GetNewPositons(IEnumerable<StockModel> actualHoldings, IEnumerable<StockDifferenceModel> overlap)
        {
            return actualHoldings.Where(actual => !overlap.Any(past => past.Ticker == actual.Ticker));
        }

        private IEnumerable<StockDifferenceModel> GetIncreasedPositons(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares > 0);
        }

        private IEnumerable<StockDifferenceModel> GetReducedPositions(IEnumerable<StockDifferenceModel> overlap)
        {
            return overlap.Where(actual => actual.DifferenceInShares < 0);
        }

        private IEnumerable<StockDifferenceModel> GetReducedToZeroPositions(IEnumerable<StockModel> pastHoldings, IEnumerable<StockDifferenceModel> overlap)
        {
            return pastHoldings
                .Where(past => !overlap.Any(actual => actual.Ticker == past.Ticker))
                .Select(past => new StockDifferenceModel
                {
                    Ticker = past.Ticker,
                    CompanyName = past.Company,
                    DifferenceInShares = -past.Shares,
                    Weight = "0%",
                });
        }

        private IEnumerable<StockDifferenceModel> GetOverlappingPositions(IEnumerable<StockModel> actualHoldings, IEnumerable<StockModel> pastHoldings)
        {
            return pastHoldings.Join(
                actualHoldings,
                past => past.Ticker,
                actual => actual.Ticker,
                (past, actual) => new StockDifferenceModel
                {
                    Ticker = actual.Ticker,
                    CompanyName = actual.Company,
                    DifferenceInShares = actual.Shares - past.Shares,
                    Weight = actual.Weight,
                });
        }
    }
}
