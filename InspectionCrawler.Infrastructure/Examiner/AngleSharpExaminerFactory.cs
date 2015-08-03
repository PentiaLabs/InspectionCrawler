using System;
using InspectionCrawler.Domain.Interfaces;

namespace InspectionCrawler.Infrastructure.Examiner
{
    public class AngleSharpExaminerFactory : IExaminerFactory
    {
        public IExaminer CreateExaminer(ILog log, Uri uri, string content)
        {
            return new AngleSharpExaminer(log, uri, content);
        }
    }
}
