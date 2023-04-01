using Stocks.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Output
{
    public interface IOutputService
    {
        public Task Output(HoldingsDifferenceModel differences, string destination);
    }
}
