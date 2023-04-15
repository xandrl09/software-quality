namespace Stocks.Services.Exceptions;

public class InvalidDownloadException : Exception
{
    public InvalidDownloadException() : base(
        ExceptionStrings.GetExceptionMessage(CustomExecption.InvalidDownload))
    {
    }

    public InvalidDownloadException(string message) : base(
        ExceptionStrings.GetExceptionMessage(CustomExecption.InvalidDownload))
    {
    }

    public InvalidDownloadException(string message, Exception innerException) : base(
        ExceptionStrings.GetExceptionMessage(CustomExecption.InvalidDownload), innerException)
    {
    }
}