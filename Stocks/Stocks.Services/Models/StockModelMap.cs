using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Models
{
    public class StockModelMap: ClassMap<StockModel>
    {
        public StockModelMap()
        {
            Map(m => m.Company).Name("company");
            Map(m => m.Ticker).Name("ticker");
            Map(m => m.Shares).Name("shares").Convert(args =>
            {
                return int.Parse(args.Row.GetField("shares").Replace(",", ""));
            });
        }
    }
}
