using System.Text;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.LogHandler
{
    public abstract class BaseLogHandler : ILogHandler
    {
        protected BaseLogHandler(LogType logLevel)
        {
            LogLevel = logLevel;
        }

        public abstract void Log(InspectorLogMessage inspectorLogMessage);
        public LogType LogLevel { get; }
        public abstract void Log(LogMessage message);
        public abstract void Flush();

        protected string StringifyLogMessage(LogMessage logMessage)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}: {1}", logMessage.LogType, logMessage.Message).AppendLine();
            if (logMessage.Uri != null)
                sb.AppendFormat("\tUri: ").Append(logMessage.Uri).AppendLine();
            if (logMessage.Exception != null)
                sb.AppendFormat("\tException: ").Append(logMessage.Exception).AppendLine();
            return sb.ToString();
        }
    }
}
