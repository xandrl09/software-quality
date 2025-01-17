﻿using Quartz;
using Stocks.Services.Diff;
using Stocks.Services.Download;
using Stocks.Services.Exceptions;
using Stocks.Services.Export;
using Stocks.Services.Files;
using Stocks.Services.Helpers;
using Stocks.Services.Models;
using Stocks.Services.Models.Configuration;
using Stocks.Services.Output;
using Stocks.Services.Parsers;

namespace Stocks.WebService.Jobs
{
    /// <summary>
    /// Class <c>StocksDiffJob</c> represents a job to be performed by the scheduler to create a daily diff of stocks.
    /// </summary>
    public class StocksDiffJob : IJob
    {
        private readonly IDownloadService _downloadService;
        private readonly IFileService _dateFileService;
        private readonly IParseService _parserService;
        private readonly IHoldingsDifferenceService _differenceService;
        private readonly Settings _settings;
        private readonly IOutputService _outputService;
        private readonly IExportService _exportService;

        public StocksDiffJob(IDownloadService downloadService,
            IFileService dateFileService,
            IParseService parserService,
            IHoldingsDifferenceService differenceService,
            Settings settings,
            IOutputService outputService,
            IExportService exportService)
        {
            _downloadService = downloadService;
            _dateFileService = dateFileService;
            _parserService = parserService;
            _differenceService = differenceService;
            _settings = settings;
            _outputService = outputService;
            _exportService = exportService;
        }

        /// <summary>
        /// Executes the job. Downloads the CSV file, parses it, compares it to the previous day's CSV file,
        /// generates an output and exports it.
        /// In case of an exception, the exception message is printed to the console.
        /// </summary>
        /// <param name="context"></param>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var csv = await _downloadService.DownloadFile(_settings.CsvUrl);

                if (string.IsNullOrEmpty(csv))
                {
                    Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.EmptyCsvFile));
                    return;
                }

                await _dateFileService.SaveContent(csv, _settings.FileExtension);

                string pathToRecentFile = PathHelper.GetDateFilePath(DateTime.Today,
                    _settings.FileNameFormat,
                    _settings.SaveDirectory,
                    _settings.FileExtension);

                var pathToOlderFile =
                    _dateFileService.GetLastAvailableFilePath(_settings.SaveDirectory, _settings.FileExtension);

                var recentHoldings = await _parserService.GetStocksAsync(pathToRecentFile);
                var pastHoldings = await _parserService.GetStocksAsync(pathToOlderFile);

                var diffResult = _differenceService.GetDifference(recentHoldings, pastHoldings);

                var htmlOutput = await _outputService.GenerateOutput(diffResult);

                await _dateFileService.SaveContent(htmlOutput, ".html");

                await _exportService.Export(htmlOutput);
            }
            catch (CsvFilePathNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException)
            {
                Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.IoException));
            }
            catch (InvalidDownloadException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (MissingFieldException)
            {
                Console.WriteLine(ExceptionStrings.GetExceptionMessage(CustomException.MissingFieldException));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}