namespace Stocks.Services.Exceptions;

public class CsvFilePathNotFoundException : Exception
{
    public CsvFilePathNotFoundException() : base(ExceptionStrings.GetExceptionMessage(CustomExecption.CsvFilePathNotFound))
    {
    }

    public CsvFilePathNotFoundException(string message) : base(ExceptionStrings.GetExceptionMessage(CustomExecption.CsvFilePathNotFound))
    {
    }

    public CsvFilePathNotFoundException(string message, Exception innerException) : base(ExceptionStrings.GetExceptionMessage(CustomExecption.CsvFilePathNotFound),
        innerException)
    {
    }
}