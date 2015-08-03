using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface ILog : ILogLevel
    {
        void Log(LogMessage message);
    }
}
