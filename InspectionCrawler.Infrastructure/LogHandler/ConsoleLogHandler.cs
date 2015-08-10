using System;
using InspectionCrawler.Domain.Model;
using InspectionCrawler.Infrastructure.Extensions;

namespace InspectionCrawler.Infrastructure.LogHandler
{
    public class ConsoleLogHandler : BaseLogHandler
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
