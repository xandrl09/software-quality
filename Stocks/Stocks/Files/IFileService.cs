using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Files
{
    public interface IFileService
    {
        public Task SaveContent(string content);
        public Task<string> LoadContent(DateTime date);
    }
}
