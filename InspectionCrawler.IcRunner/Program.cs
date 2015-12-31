using System;
using System.Linq;
using InspectionCrawler.Application.Service;
using InspectionCrawler.Domain.Model;
using CommandLine;

namespace InspectionCrawler.IcRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = GetOptions(args);
            if (options == null) return;

            var crawlService = new CrawlService(LogType.Info)
            {
                SlowPageDetector =
                {
                    IsEnabled = options.SlowPageDetectorEnabled,
                    Milliseconds = options.SlowPageDetectorMilliseconds
                },
                LargePageDetector =
                {
                    IsEnabled = options.LargePageDetectorEnabled,
                    ByteSize = options.LargePageDetectorByteSizes
                },
                ErrorDetector =
                {
                    IsEnabled = options.ErrorDetectorEnabled,
                    CheckExternalLinks = options.ErrorDetectorCheckExternalLinks,
                    TreatRedirectsAsErrors = options.ErrorDetectorTreatRedirectsAsErrors
                },
                AllReferencesCollector =
                {
                    IsEnabled = options.AllReferencesCollectorEnabled,
                    Targets = options.AllReferencesCollectorTargets.ToList()
                },
                SchemeCollector =
                {
                    IsEnabled = options.SchemeCollectorEnabled,
                    Schemes = options.SchemeCollectorSchemes.ToList()
                },
                ExternalLinksCollector =
                {
                    IsEnabled = options.ExternalLinksCollectorEnabled,
                    Ignore = options.ExternalLinksCollectorIgnore.ToList()
                }
            };

            if(options.LogFile != null)
                crawlService.LogToFile(options.LogFile);

            var report = crawlService.Crawl(options.Url);

            Console.WriteLine($"Crawled {report.NumberOfPagesCrawled} pages in {report.Elapsed.TotalSeconds} seconds");
        }

        private static Options GetOptions(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            var wasSuccessful = true;
            var options = result.MapResult(o => o, errors => { wasSuccessful = false; return null; });

            if (!wasSuccessful) return null;

            if (options.Url.Scheme != Uri.UriSchemeHttp && options.Url.Scheme != Uri.UriSchemeHttps)
            {
                Console.WriteLine("The specified url must have http or https as scheme");
                return null;
            }

            return options;
        }
    }
}
