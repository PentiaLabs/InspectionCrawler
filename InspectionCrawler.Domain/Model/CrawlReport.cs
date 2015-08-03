using System;

namespace InspectionCrawler.Domain.Model
{
    public sealed class CrawlReport
    {
        internal CrawlReport(int numberOfPagesCrawled, DateTime crawlStartedOnUtc, DateTime crawlCompletedOnUtc)
        {
            NumberOfPagesCrawled = numberOfPagesCrawled;
            CrawlStartedOnUtc = crawlStartedOnUtc;
            CrawlCompletedOnUtc = crawlCompletedOnUtc;
        }

        public int NumberOfPagesCrawled { get; private set; }
        public DateTime CrawlStartedOnUtc { get; }
        public DateTime CrawlCompletedOnUtc { get; }
        public TimeSpan Elapsed => CrawlCompletedOnUtc.Subtract(CrawlStartedOnUtc);
    }
}
