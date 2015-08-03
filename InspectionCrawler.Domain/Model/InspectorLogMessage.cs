using System;
using InspectionCrawler.Domain.Interfaces;

namespace InspectionCrawler.Domain.Model
{
    public sealed class InspectorLogMessage
    {
        public InspectorLogMessage(IInspector inspector, LogMessage logMessage)
        {
            if (inspector == null) throw new ArgumentNullException(nameof(inspector));

            Inspector = inspector;
            LogMessage = logMessage;
        }

        public IInspector Inspector { get; }
        public LogMessage LogMessage { get; }
    }
}
