using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Models.Configuration
{
    /// <summary>
    /// Class <c>SmtpCredentials</c> represents the credentials for the SMTP server.
    /// </summary>
    public class SmtpCredentials
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
