using Stocks.Services.Models;

namespace Stocks.Services.Output
{
    /// <summary>
    /// Interface <c>IOutputService</c> represents a service for generating output.
    /// </summary>
    public interface IOutputService
    {
        public Task<string> GenerateOutput(HoldingsDifferenceModel differences);
    }
}