using HtmlBuilders;
using Microsoft.Extensions.Configuration;
using Stocks.Services.Helpers;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using System.Text.Encodings.Web;

namespace Stocks.Services.Output
{
    public class HtmlOutputService : IOutputService
    {
        private readonly Settings _settings;

        public HtmlOutputService(IConfiguration configuration)
        {
            _settings = Settings.Get(configuration);
        }

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

        private HtmlTag CreateHtml()
        {
            var html = new HtmlTag("html");
            return html;
        }

        private HtmlTag CreateHead()
        {
            var head = new HtmlTag("head");
            return head;
        }

        private HtmlTag CreateBody()
        {
            var body = new HtmlTag("body");
            return body;
        }

        private HtmlTag CreateHeader()
        {
            var html = new HtmlTag("div");
            var headerText = new HtmlTag("h1").Append($"Stocks difference for {DateTime.Today.ToShortDateString()}");
            html = html.Append(headerText);

            return html;
        }

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

        private string GetDestinationFilename(string destination)
        {
            var filename = $"diff_{PathHelper.FormatDateTime(DateTime.Today, _settings.FileNameFormat)}.html";
            return Path.Combine(destination, filename);
        }
    }
}