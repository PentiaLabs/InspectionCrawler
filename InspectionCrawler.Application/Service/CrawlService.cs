using System;
using System.Collections.Generic;
using InspectionCrawler.Application.Model;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using InspectionCrawler.Infrastructure.AbotCrawler;
using InspectionCrawler.Infrastructure.AngleSharpExamine;
using InspectionCrawler.Infrastructure.Inspectors;
using InspectionCrawler.Infrastructure.LogHandlers;

namespace InspectionCrawler.Application.Service
{
    public sealed class CrawlService
    {
        private readonly LogType _logLevel;
        private ILogHandler _log;

        public CrawlService(LogType logLevel)
        {
            _logLevel = logLevel;
            _log = new ConsoleLogHandler(_logLevel);
            SlowPageDetector = new SlowPageDetectorSettings();
            LargePageDetector = new LargePageDetectorSettings();
            ErrorDetector = new ErrorDetectorSettings();
            AllReferencesCollector = new AllReferencesCollectorSettings();
            SchemeCollector = new SchemeCollectorSettings();
            ExternalLinksCollector = new ExternalLinksCollectorSettings();
        }

        public CrawlReport Crawl(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var crawler = new Crawler(new AbotCrawl(_log), _log, new AngleSharpExaminerFactory(), GetInspectors());
            return crawler.Crawl(uri);
        }

        public SlowPageDetectorSettings SlowPageDetector { get; }
        public LargePageDetectorSettings LargePageDetector { get; }
        public ErrorDetectorSettings ErrorDetector { get; }
        public AllReferencesCollectorSettings AllReferencesCollector { get; }
        public SchemeCollectorSettings SchemeCollector { get; }
        public ExternalLinksCollectorSettings ExternalLinksCollector { get; }

        public CrawlService LogToFile(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if(string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path is empty or whitespace", nameof(path));

            _log = new FileLogHandler(_logLevel, path);

            return this;
        }

        private IEnumerable<IInspector> GetInspectors()
        {
            var inspectors = new List<IInspector>();

            if (SlowPageDetector.IsEnabled)
                inspectors.Add(new SlowPageDetector(_log, SlowPageDetector.Milliseconds));
            if (LargePageDetector.IsEnabled)
                inspectors.Add(new LargePageDetector(_log, LargePageDetector.ByteSize));
            if(ErrorDetector.IsEnabled)
                inspectors.Add(new ErrorDetector(_log, ErrorDetector.CheckExternalLinks, ErrorDetector.TreatRedirectsAsErrors));
            if(AllReferencesCollector.IsEnabled)
                inspectors.Add(new AllReferencesCollector(_log, AllReferencesCollector.Targets));
            if(SchemeCollector.IsEnabled)
                inspectors.Add(new SchemeCollector(_log, SchemeCollector.Schemes));
            if(ExternalLinksCollector.IsEnabled)
                inspectors.Add(new ExternalLinksCollector(_log, ExternalLinksCollector.Ignore));

            return inspectors;
        }
    }
}
