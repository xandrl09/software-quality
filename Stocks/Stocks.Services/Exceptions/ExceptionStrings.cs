using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Exceptions
{
    public enum CustomException
    {
        CsvFilePathNotFound,
        InvalidDownload,
        EmptyCsvFile,
        IoException,
        MissingFieldException,
        UnknownException
    }

    public static class ExceptionStrings
    {
        public static string GetExceptionMessage(CustomException e)
        {
            var ExceptionsDictionary = new Dictionary<CustomException, string>()
            {
                {CustomException.CsvFilePathNotFound, "Could not find path to the last available csv file."},
                {CustomException.InvalidDownload, "Could not download file from API. Check your internet connection and try again." },
                {CustomException.EmptyCsvFile, "Downloaded csv file is empty."},
                {CustomException.IoException , "AN I/O error occurred while opening the file."},
                {CustomException.MissingFieldException , "Unable to parse provided csv file. There is a missing field. Please check the file and try again."},
                {CustomException.UnknownException, "Unknown Exception occured." }
            };
            return ExceptionsDictionary.GetValueOrDefault(e);
        }

    }
}
