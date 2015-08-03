using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface ILogLevel
    {
        LogType LogLevel { get; }
    }
}
