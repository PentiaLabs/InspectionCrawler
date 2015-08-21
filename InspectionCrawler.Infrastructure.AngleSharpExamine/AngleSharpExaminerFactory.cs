using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.AngleSharpExamine
{
    public class AngleSharpExaminerFactory : IExaminerFactory
    {
        public IExaminer CreateExaminer(ILog log, Page page)
        {
            return new AngleSharpExaminer(log, page.Uri, page.Content);
        }
    }
}
