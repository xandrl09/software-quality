namespace Stocks.Services.Exceptions;

public class CsvFilePathNotFoundException : Exception
{
    public CsvFilePathNotFoundException() : base("Could not find path to the last available csv file.")
    {
    }

    public CsvFilePathNotFoundException(string message) : base("Could not find path to the last available csv file.")
    {
    }

    public CsvFilePathNotFoundException(string message, Exception innerException) : base("Could not find path to the last available csv file.",
        innerException)
    {
    }
}