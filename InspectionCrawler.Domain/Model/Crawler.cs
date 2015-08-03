using System;
using InspectionCrawler.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InspectionCrawler.Domain.Model
{
    public sealed class Crawler
    {
        private readonly ICrawl _crawl;
        private readonly ILogHandler _log;
        private readonly IExaminerFactory _examinerFactory;
        private readonly List<IInspector> _inspectors;
        private int _numberOfPagesCrawled;

        public Crawler(ICrawl crawl, ILogHandler log, IExaminerFactory examinerFactory, IEnumerable<IInspector> inspectors)
        {
            if (crawl == null) throw new ArgumentNullException(nameof(crawl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (examinerFactory == null) throw new ArgumentNullException(nameof(examinerFactory));
            if (inspectors == null) throw new ArgumentNullException(nameof(inspectors));

            _crawl = crawl;
            _log = log;
            _examinerFactory = examinerFactory;
            _inspectors = inspectors.ToList();
        }

        public CrawlReport Crawl(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            var crawlStartedOnUtc = DateTime.UtcNow;

            CallCrawlStartingOnInspectors();

            _crawl.Crawl(uri, Callback);

            CallCrawlCompletedOnInspectors();

            _log.Flush();

            var crawlCompletedOnUtc = DateTime.UtcNow;

            var report = new CrawlReport(_numberOfPagesCrawled, crawlStartedOnUtc, crawlCompletedOnUtc);

            _numberOfPagesCrawled = 0;

            return report;
        }

        private void Callback(Page page)
        {
            Interlocked.Increment(ref _numberOfPagesCrawled);

            try
            {
                page.Examiner = _examinerFactory.CreateExaminer(_log, page.Uri, page.Content);
            }
            catch (Exception exception)
            {
                _log.Log(new LogMessage(LogType.Error, "Uncaught exception when creating examiner", exception, page.Uri));
                return;
            }

            foreach (var inspector in _inspectors)
            {
                try
                {
                    inspector.InspectPage(page);
                }
                catch (Exception exception)
                {
                    _log.Log(new InspectorLogMessage(inspector, new LogMessage(LogType.Error, "Uncaught exception when calling InspectPage", exception, page.Uri)));
                }
            }
        }

        private void CallCrawlStartingOnInspectors()
        {
            foreach (var inspector in _inspectors)
            {
                try
                {
                    inspector.CrawlStarting();
                }
                catch (Exception exception)
                {
                    _log.Log(new InspectorLogMessage(inspector, new LogMessage(LogType.Error, "Uncaught exception when calling CrawlStarting", exception)));
                }
            }
        }

        private void CallCrawlCompletedOnInspectors()
        {
            foreach (var inspector in _inspectors)
            {
                try
                {
                    inspector.CrawlCompleted();
                }
                catch (Exception exception)
                {
                    _log.Log(new InspectorLogMessage(inspector, new LogMessage(LogType.Error, "Uncaught exception when calling CrawlCompleted", exception)));
                }
            }
        }
    }
}
