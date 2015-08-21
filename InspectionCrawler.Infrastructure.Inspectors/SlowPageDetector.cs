using System;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspectors
{
    public class SlowPageDetector : Inspector
    {
        private readonly int _milliseconds;

        public SlowPageDetector(IInspectorLog log, int milliseconds) : base(log)
        {
            _milliseconds = milliseconds;
        }

        public override string Name => "Slow page detector";

        public override void InspectPage(Page page)
        {
            var timeSpan = page.RequestCompletedOnUtc - page.RequestStartedOnUtc;
            var loadTime = (int) timeSpan.TotalMilliseconds;
            if (loadTime  > _milliseconds)
            {
                Error(GetMessage(page.Uri, loadTime));
                return;
            }

            if (IsInfoEnabled)
                Info(GetMessage(page.Uri, loadTime));
        }

        private string GetMessage(Uri uri, int loadTime)
        {
            return $"{uri} took {loadTime} milliseconds";
        }
    }
}
