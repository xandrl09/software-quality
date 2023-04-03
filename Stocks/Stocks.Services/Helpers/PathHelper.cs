namespace Stocks.Services.Helpers
{
    public static class PathHelper
    {
        public static string FormatDateTime(DateTime date, 
            string format)
        {
            return string.Format("{0:" + format + "}", date);
        }

        public static string GetDateFilePath (DateTime date,
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
