namespace Stocks.Services.Exceptions;

/// <summary>
/// Class <c>CsvFilePathNotFoundException</c> represents the exception that is thrown when the CSV file path is not found.
/// </summary>
public class CsvFilePathNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvFilePathNotFoundException"/>.
    /// </summary>
    public CsvFilePathNotFoundException() : base(
        ExceptionStrings.GetExceptionMessage(CustomException.CsvFilePathNotFound))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvFilePathNotFoundException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public CsvFilePathNotFoundException(string message) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.CsvFilePathNotFound))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CsvFilePathNotFoundException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public CsvFilePathNotFoundException(string message, Exception innerException) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.CsvFilePathNotFound),
        innerException)
    {
    }
}