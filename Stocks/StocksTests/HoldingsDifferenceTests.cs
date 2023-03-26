﻿using Stocks.Services.Models;

namespace Stocks.Services.Diff;

public class HoldingsDifferenceTests
{
    private HoldingsDifferenceService _diffMaker;
    private List<StockModel> _oldReport;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _diffMaker = new HoldingsDifferenceService();
        _oldReport = new List<StockModel> {
            new StockModel { Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "50%" },
            new StockModel { Ticker = "GOOG", Company = "Google", Shares = 10, Weight = "50%" },
        };
    }

    [Test]
    public void GetResult_NewStockAdded_ReturnsSucces()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "25%" },
            new StockModel { Ticker = "GOOG", Company = "Google", Shares = 10, Weight = "25%" },
            new StockModel { Ticker = "TSLA", Company = "Tesla", Shares = 20, Weight = "50%" }
        };

        // act
        var result = _diffMaker.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.NewPositions.Count, Is.EqualTo(1));
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(0));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.FirstOrDefault()?.Ticker, Is.EqualTo("TSLA"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Company, Is.EqualTo("Tesla"));
            Assert.That(result.NewPositions.FirstOrDefault()?.Shares, Is.EqualTo(20));
            Assert.That(result.NewPositions.FirstOrDefault()?.Weight, Is.EqualTo("50%"));
        });
    }

    [Test]
    public void GetResult_StockPositionIncreased_ReturnsSucces()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "10%" },
            new StockModel { Ticker = "GOOG", Company = "Google", Shares = 90, Weight = "90%" },
        };

        // act
        var result = _diffMaker.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(1));
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Ticker, Is.EqualTo("GOOG"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.CompanyName, Is.EqualTo("Google"));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(80));
            Assert.That(result.IncreasedPositons.FirstOrDefault()?.Weight, Is.EqualTo("90%"));
        });
    }

    [Test]
    public void GetResult_StockPositionReduced_ReturnsSucces()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "83%" },
            new StockModel { Ticker = "GOOG", Company = "Google", Shares = 2, Weight = "17%" },
        };

        // act
        var result = _diffMaker.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(1));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Ticker, Is.EqualTo("GOOG"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.CompanyName, Is.EqualTo("Google"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(-8));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Weight, Is.EqualTo("17%"));
        });
    }

    [Test]
    public void GetResult_StockPositionReducedToZero_ReturnsSucces()
    {
        // arrange
        var newReport = new List<StockModel> {
            new StockModel { Ticker = "MSFT", Company = "Microsoft", Shares = 10, Weight = "83%" }
        };

        // act
        var result = _diffMaker.GetDifference(newReport, _oldReport);

        // assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ReducedPositions.Count, Is.EqualTo(1));
            Assert.That(result.IncreasedPositons.Count, Is.EqualTo(0));
            Assert.That(result.NewPositions.Count, Is.EqualTo(0));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Ticker, Is.EqualTo("GOOG"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.CompanyName, Is.EqualTo("Google"));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.DifferenceInShares, Is.EqualTo(-10));
            Assert.That(result.ReducedPositions.FirstOrDefault()?.Weight, Is.EqualTo("0%"));
        });
    }
}