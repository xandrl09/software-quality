using Stocks.Services.Models;

namespace Stocks.Services.Output
{
    public interface IOutputService
    {
        public Task Output(HoldingsDifferenceModel differences, string destination);
    }
}
