namespace Stocks.Services.Exceptions;

public class InvalidDownloadException : Exception
{
    public InvalidDownloadException() : base(
        "Could not download file from API. Check your internet connection and try again.")
    {
    }

    public InvalidDownloadException(string message) : base(
        "Could not download file from API. Check your internet connection and try again.")
    {
    }

    public InvalidDownloadException(string message, Exception innerException) : base(
        "Could not download file from API. Check your internet connection and try again.", innerException)
    {
    }
}