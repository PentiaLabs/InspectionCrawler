namespace InspectionCrawler.Domain.Interfaces
{
    public interface ILogHandler : IInspectorLog, ILog
    {
        void Flush();
    }
}
