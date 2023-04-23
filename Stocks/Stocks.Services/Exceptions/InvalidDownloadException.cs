namespace Stocks.Services.Exceptions;

public class InvalidDownloadException : Exception
{
    public InvalidDownloadException() : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload))
    {
    }

    public InvalidDownloadException(string message) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload))
    {
    }

    public InvalidDownloadException(string message, Exception innerException) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload), innerException)
    {
    }
}