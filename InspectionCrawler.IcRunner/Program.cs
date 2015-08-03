using System;
using InspectionCrawler.Application.Service;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.IcRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify a url and optionally a file name");
                return;
            }

            if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
            {
                Console.WriteLine("The specified url is not valid");
                return;
            }

            var uri = new Uri(args[0]);
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                Console.WriteLine("The specified url must have http or https as scheme");
                return;
            }

            var path = args.Length == 2 ? args[1] : null;

            var crawlService = new CrawlService(LogType.Info)
            {
                SlowPageDetector = { IsEnabled = false, Milliseconds = 400 },
                LargePageDetector = { IsEnabled = false, ByteSize = 1024*1024 },
                ErrorDetector = { IsEnabled = true, CheckExternalLinks = true, TreatRedirectsAsErrors = false },
                AllReferencesCollector = { IsEnabled = false },
                SchemeCollector = { IsEnabled = false, Schemes = { Uri.UriSchemeMailto } },
                ExternalLinksCollector = { IsEnabled = false }
            };

            if(path != null)
                crawlService.LogToFile(path);

            var report = crawlService.Crawl(uri);

            Console.WriteLine($"Crawled {report.NumberOfPagesCrawled} pages in {report.Elapsed.TotalSeconds} seconds");
        }
    }
}
