using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Helpers
{
    public static class PathHelper
    {
        public static string GetDateFilePath (DateTime date,
            string format,
            string directory,
            string extension)
        {
            string path = string.Format("{0:" + format + "}", date);
            path = Path.Join(directory, path + extension);
            return path;
        }
    }
}
