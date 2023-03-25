using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Models
{
    public class StockModel
    {
        public DateTime Date { get; init; }
        public string Company { get; init; }
        public string Ticker { get; init; }
        public int Shares { get; init; } 
        public string MarketValue { get; init; }
    }
}
