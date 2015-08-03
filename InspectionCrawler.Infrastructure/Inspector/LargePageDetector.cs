using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspector
{
    public class LargePageDetector : BaseInspector
    {
        private readonly int _byteSize;

        public LargePageDetector(IInspectorLog log, int byteSize) : base(log)
        {
            _byteSize = byteSize;
        }

        public override string Name => "Large page detector";

        public override void InspectPage(Page page)
        {
            if (page.ContentByteSize > _byteSize)
            {
                Error(GetMessage(page));
                return;
            }

            if (IsInfoEnabled)
                Info(GetMessage(page));
        }

        public override void CrawlStarting() { }

        public override void CrawlCompleted() { }

        private string GetMessage(Page page)
        {
            return $"{page.Uri} is {page.ContentByteSize} bytes";
        }
    }
}
