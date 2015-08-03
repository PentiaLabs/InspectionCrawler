using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface IInspector
    {
        string Name { get; }
        void CrawlStarting();
        void InspectPage(Page page);
        void CrawlCompleted();
    }
}
