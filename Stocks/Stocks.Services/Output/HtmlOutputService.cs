using HtmlBuilders;
using Microsoft.Extensions.Configuration;
using Stocks.Services.Helpers;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using System.Text.Encodings.Web;

namespace Stocks.Services.Output
{
    /// <summary>
    /// Class <c>HtmlOutputService</c> represents a service for generating output in HTML format.
    /// </summary>
    public class HtmlOutputService : IOutputService
    {
        private readonly Settings _settings;

        /// <summary>
        /// Creates a new instance of <see cref="HtmlOutputService"/>.
        /// </summary>
        /// <param name="settings">The settings of the application.</param>
        public HtmlOutputService(Settings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Generates the output in HTML format.
        /// </summary>
        /// <param name="differences">The differences for holdings between two dates.</param>
        /// <returns>The output in HTML format.</returns>
        public async Task<string> GenerateOutput(HoldingsDifferenceModel differences)
        {
            var html = CreateHtml();
            html = html.Append(CreateHead());

            var body = CreateBody();

            var contents = new HtmlTag("div");

            contents = contents.Append(CreateHeader());
            contents = contents.Append(CreateTable("New positions", differences.NewPositions));
            contents = contents.Append(CreateTable("Increased positions", differences.IncreasedPositons));
            contents = contents.Append(CreateTable("Reduced positions", differences.ReducedPositions));

            body = body.Append(contents);

            html = html.Append(body);

            return html.ToHtmlString();
        }

        /// <summary>
        /// Creates the HTML tag.
        /// </summary>
        /// <returns>The HTML tag.</returns>
        private HtmlTag CreateHtml()
        {
            var html = new HtmlTag("html");
            return html;
        }

        /// <summary>
        /// Creates the head tag.
        /// </summary>
        /// <returns>The head tag.</returns>
        private HtmlTag CreateHead()
        {
            var head = new HtmlTag("head");
            return head;
        }

        /// <summary>
        /// Creates the body tag.
        /// </summary>
        /// <returns>The body tag.</returns>
        private HtmlTag CreateBody()
        {
            var body = new HtmlTag("body");
            return body;
        }

        /// <summary>
        /// Creates the header tag.
        /// </summary>
        /// <returns>The header tag.</returns>
        private HtmlTag CreateHeader()
        {
            var html = new HtmlTag("div");
            var headerText = new HtmlTag("h1").Append($"Stocks difference for {DateTime.Today.ToShortDateString()}");
            html = html.Append(headerText);

            return html;
        }

        /// <summary>
        /// Creates a table for the differences.
        /// </summary>
        /// <param name="header">The header of the table.</param>
        /// <param name="differences">The differences.</param>
        /// <typeparam name="T">The type of the differences.</typeparam>
        /// <returns>The table of the differences with the header.</returns>
        private HtmlTag CreateTable<T>(string header, IEnumerable<T> differences)
        {
            var html = new HtmlTag("div");
            html = html.Append(new HtmlTag("h2").Append(header));

            if (!differences.Any())
            {
                html = html.Append(new HtmlTag("i").Append("No data"));
                return html;
            }

            var table = new HtmlTag("table");

            var headerRow = new HtmlTag("tr");
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == nameof(StockModel.Cusip)) continue;

                headerRow = headerRow.Append(new HtmlTag("th").Append(prop.Name));
            }

            table = table.Append(headerRow);

            foreach (var diff in differences)
            {
                var row = new HtmlTag("tr");

                foreach (var prop in props)
                {
                    if (prop.Name == nameof(StockModel.Cusip)) continue;

                    var val = prop.GetValue(diff)?.ToString() ?? "";
                    row = row.Append(new HtmlTag("td").Append(val));
                }

                table = table.Append(row);
            }

            html = html.Append(table);

            return html;
        }
    }
}