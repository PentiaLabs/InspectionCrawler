using System;
using InspectionCrawler.Domain.Extensions;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.LogHandlers
{
    public class ConsoleLogHandler : LogHandler
    {
        public ConsoleLogHandler(LogType logLevel) : base(logLevel) { }

        public override void Log(InspectorLogMessage message)
        {
            if (LogLevel.ShouldLog(message.LogMessage.LogType))
                Console.WriteLine("{0} >> {1}", message.Inspector.Name, StringifyLogMessage(message.LogMessage));
        }

        public override void Log(LogMessage message)
        {
            if (LogLevel.ShouldLog(message.LogType))
                Console.Write(StringifyLogMessage(message));
        }

        public override void Flush()
        {
            Console.Out.Flush();
        }
    }
}
