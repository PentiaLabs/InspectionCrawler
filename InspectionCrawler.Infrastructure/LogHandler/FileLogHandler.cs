using System.Collections.Concurrent;
using System.IO;
using InspectionCrawler.Domain.Model;
using InspectionCrawler.Infrastructure.Extensions;

namespace InspectionCrawler.Infrastructure.LogHandler
{
    public class FileLogHandler : BaseLogHandler
    {
        private readonly string _path;
        private readonly ConcurrentDictionary<string, ConcurrentBag<LogMessage>> _inspectorLogMessages;
        private readonly ConcurrentBag<LogMessage> _logMessages;

        public FileLogHandler(LogType logLevel, string path) : base(logLevel)
        {
            _path = path;
            _inspectorLogMessages = new ConcurrentDictionary<string, ConcurrentBag<LogMessage>>();
            _logMessages = new ConcurrentBag<LogMessage>();
        }

        public override void Log(InspectorLogMessage message)
        {
            if (!LogLevel.ShouldLog(message.LogMessage.LogType)) return;

            var bag = _inspectorLogMessages.GetOrAdd(message.Inspector.Name, s => new ConcurrentBag<LogMessage>());
            bag.Add(message.LogMessage);
        }

        public override void Log(LogMessage message)
        {
            if (LogLevel.ShouldLog(message.LogType))
                _logMessages.Add(message);
        }

        public override void Flush()
        {
            using (var writer = new StreamWriter(_path))
            {
                writer.WriteLine("*** Crawler logs ***");
                foreach (var logMessage in _logMessages)
                {
                    writer.WriteLine(StringifyLogMessage(logMessage));
                }

                writer.WriteLine();

                writer.WriteLine("*** Inspector logs ***");
                foreach (var inspectorLogMessage in _inspectorLogMessages)
                {
                    writer.WriteLine("Inspector: " + inspectorLogMessage.Key);
                    foreach (var logMessage in inspectorLogMessage.Value)
                    {
                        writer.WriteLine(StringifyLogMessage(logMessage));
                    }
                }
            }
        }
    }
}
