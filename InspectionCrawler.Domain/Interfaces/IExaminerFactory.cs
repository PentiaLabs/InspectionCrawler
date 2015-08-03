using System;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface IExaminerFactory
    {
        IExaminer CreateExaminer(ILog log, Uri uri, string content);
    }
}
