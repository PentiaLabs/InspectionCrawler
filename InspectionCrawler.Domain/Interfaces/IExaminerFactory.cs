using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface IExaminerFactory
    {
        IExaminer CreateExaminer(ILog log, Page page);
    }
}
