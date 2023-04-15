using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Exceptions
{
    public enum CustomExecption
    {
        CsvFilePathNotFound,
        InvalidDownload,
        EmptyCsvFile,
        UnknownException
    }

    public static class ExceptionStrings
    {
        public static string GetExceptionMessage(CustomExecption e)
        {
            var ExceptionsDictionary = new Dictionary<CustomExecption, string>()
            {
                {CustomExecption.CsvFilePathNotFound, "Could not find path to the last available csv file."},
                {CustomExecption.InvalidDownload, "Could not download file from API. Check your internet connection and try again." },
                {CustomExecption.EmptyCsvFile, "Downloaded csv file is empty."},
                {CustomExecption.UnknownException, "Unknown Exception occured." }
            };
            return ExceptionsDictionary.GetValueOrDefault(e);
        }

    }
}
