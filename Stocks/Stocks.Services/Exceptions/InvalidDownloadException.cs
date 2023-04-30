namespace Stocks.Services.Exceptions;

/// <summary>
/// Class <c>InvalidDownloadException</c> represents the exception that is thrown when the download fails.
/// </summary>
public class InvalidDownloadException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="InvalidDownloadException"/>.
    /// </summary>
    public InvalidDownloadException() : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="InvalidDownloadException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidDownloadException(string message) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="InvalidDownloadException"/>.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidDownloadException(string message, Exception innerException) : base(
        ExceptionStrings.GetExceptionMessage(CustomException.InvalidDownload), innerException)
    {
    }
}