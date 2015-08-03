using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface IInspectorLog : ILogLevel
    {
        void Log(InspectorLogMessage message);
    }
}
