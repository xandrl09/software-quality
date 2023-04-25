using Stocks.Services.Models;

namespace Stocks.Services.Output
{
    public interface IOutputService
    {
        public Task<string> GenerateOutput(HoldingsDifferenceModel differences);
    }
}