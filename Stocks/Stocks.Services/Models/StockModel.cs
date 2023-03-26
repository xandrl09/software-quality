using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Models
{
    public class StockModel
    {
        public string Company { get; init; }
        public string Ticker { get; init; }
        public int Shares { get; init; }
    }
}
