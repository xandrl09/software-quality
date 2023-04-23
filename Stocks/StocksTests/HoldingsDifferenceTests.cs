using FakeItEasy;
using Stocks.Services.Models;

namespace Stocks.Services.Diff;

public class HoldingsDifferenceTests
{
    private HoldingsDifferenceService _holdingsDifferenceService;
    private List<StockModel> _oldReport;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _holdingsDifferenceService = new HoldingsDifferenceService();
        _oldReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "15%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 30, Weight = "30%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "10%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 25, Weight = "25%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 20, Weight = "20%" },
        };
    }

    [Test]
    public void GetDifference_NewStockAdded_ReturnsNewPositon()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "8%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 30, Weight = "15%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "5%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 25, Weight = "12%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 20, Weight = "10%" },
            new StockModel { Cusip = "88160R101", Ticker = "TSLA", Company = "Tesla", Shares = 100, Weight = "50%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.NewPositions.Count, Is.EqualTo(1));
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(0));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.FirstOrDefault()?.Ticker, Is.EqualTo("TSLA"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Company, Is.EqualTo("Tesla"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Shares, Is.EqualTo(100));
            Assert.That(result.NewPositions.FirstOrDefault()?.Weight, Is.EqualTo("50%"));
        });
    }

    [Test]
    public void GetDifference_StockPositionIncreased_ReturnsIncreasedPosition()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "12%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 60, Weight = "46%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "7%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 25, Weight = "20%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 20, Weight = "15%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(1));
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Ticker, Is.EqualTo("GOOG"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.CompanyName, Is.EqualTo("Google"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(30));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.PercentageDifferenceInShares, Is.EqualTo(100));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Weight, Is.EqualTo("46%"));
        });
    }

    [Test]
    public void GetDifference_StockPositionReduced_ReturnsReducedPosition()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "17%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 30, Weight = "33%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "11%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 25, Weight = "28%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 10, Weight = "11%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(1));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Ticker, Is.EqualTo("NVDA"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.CompanyName, Is.EqualTo("NVIDIA"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(-10));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.PercentageDifferenceInShares, Is.EqualTo(-50));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Weight, Is.EqualTo("11%"));
        });
    }

    [Test]
    public void GetDifference_StockPositionReducedToZero_ReturnsReducedPosition()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "19%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 30, Weight = "38%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "12%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 25, Weight = "31%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(1));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Ticker, Is.EqualTo("NVDA"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.CompanyName, Is.EqualTo("NVIDIA"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(-20));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.PercentageDifferenceInShares, Is.EqualTo(-100));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Weight, Is.EqualTo("0%"));
        });
    }

     [Test]
    public void GetDifference_StockPositionReducedAndIncreased_ReturnsCorrectPositions()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "10%" },
            new StockModel { Cusip = "38259P706", Ticker = "GOOG", Company = "Google", Shares = 50, Weight = "50%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 15, Weight = "15%" },
            new StockModel { Cusip = "22291829E", Ticker = "AAPL", Company = "Apple", Shares = 20, Weight = "20%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 5, Weight = "5%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(2));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Ticker, Is.EqualTo("GOOG"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.CompanyName, Is.EqualTo("Google"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(20));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.PercentageDifferenceInShares, Is.EqualTo(66.67));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Weight, Is.EqualTo("50%"));

            Assert.That(result.ReducedPositions.Count, Is.EqualTo(3));
            Assert.That(result.ReducedPositions.ElementAt(0).Ticker, Is.EqualTo("MSFT"));
            Assert.That(result.ReducedPositions.ElementAt(0).CompanyName, Is.EqualTo("Microsoft"));
            Assert.That(result.ReducedPositions.ElementAt(0).DifferenceInShares, Is.EqualTo(-5));
            Assert.That(result.ReducedPositions.ElementAt(0).PercentageDifferenceInShares, Is.EqualTo(-33.33));
            Assert.That(result.ReducedPositions.ElementAt(0).Weight, Is.EqualTo("10%"));

            Assert.That(result.ReducedPositions.ElementAt(1).Ticker, Is.EqualTo("AAPL"));
            Assert.That(result.ReducedPositions.ElementAt(1).CompanyName, Is.EqualTo("Apple"));
            Assert.That(result.ReducedPositions.ElementAt(1).DifferenceInShares, Is.EqualTo(-5));
            Assert.That(result.ReducedPositions.ElementAt(1).PercentageDifferenceInShares, Is.EqualTo(-20));
            Assert.That(result.ReducedPositions.ElementAt(1).Weight, Is.EqualTo("20%"));

            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void GetDifference_TwoPositionsRemovedAndOneAdded_ReturnsCorrectResults()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Cusip = "594918104", Ticker = "MSFT", Company = "Microsoft", Shares = 15, Weight = "18%" },
            new StockModel { Cusip = "123456789", Ticker = "AMZN", Company = "Amazon", Shares = 10, Weight = "12%" },
            new StockModel { Cusip = "01KW31203", Ticker = "NVDA", Company = "NVIDIA", Shares = 20, Weight = "23%" },
            new StockModel { Cusip = "123456780", Ticker = "FB", Company = "Facebook", Shares = 40, Weight = "47%" },
        };

        // act
        var result = _holdingsDifferenceService.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            // check for removed positions
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(2));
            Assert.That(result.ReducedPositions.Any(p => p.Ticker == "GOOG"), Is.True);
            Assert.That(result.ReducedPositions.Any(p => p.Ticker == "AAPL"), Is.True);

            // check for added positions
            Assert.That(result.NewPositions.Count, Is.EqualTo(1));
            Assert.That(result.NewPositions.FirstOrDefault()?.Ticker, Is.EqualTo("FB"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Company, Is.EqualTo("Facebook"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Shares, Is.EqualTo(40));
            Assert.That(result.NewPositions.FirstOrDefault()?.Weight, Is.EqualTo("47%"));

        });
    }

    [Test]
    public void GetDifference_FakeItEasy_FunctionCalled()
    {
        // arrange
        var fakeDifferenceService = A.Fake<IHoldingsDifferenceService>();
        var fakeStock = A.Fake<List<StockModel>>();
        fakeStock.Add(A.Fake<StockModel>());

        // act
        fakeDifferenceService.GetDifference(fakeStock, _oldReport);

        // assert
        A.CallTo(() => fakeDifferenceService.GetDifference(fakeStock, _oldReport)).MustHaveHappened();

    }
}