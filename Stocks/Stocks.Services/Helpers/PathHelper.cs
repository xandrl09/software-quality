namespace Stocks.Services.Helpers
{
    /// <summary>
    /// Class <c>PathHelper</c> provides helper methods for working with file paths.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Formats the date time.
        /// </summary>
        /// <param name="date">Date to be formatted.</param>
        /// <param name="format">Format of the date.</param>
        /// <returns>The formatted date.</returns>
        public static string FormatDateTime(DateTime date,
            string format)
        {
            return string.Format("{0:" + format + "}", date);
        }

        /// <summary>
        /// Builds the path to the file with date as the name.
        /// </summary>
        /// <param name="date">Date to be used in the file name.</param>
        /// <param name="format">Format of the date.</param>
        /// <param name="directory">Directory of the file.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <returns>The path to the file.</returns>
        public static string GetDateFilePath(DateTime date,
            string format,
            string directory,
            string extension)
        {
            string path = FormatDateTime(date, format);
            path = Path.Join(directory, path + extension);
            return path;
        }
    }
}