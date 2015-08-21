using System;
using InspectionCrawler.Domain.Extensions;
using InspectionCrawler.Domain.Interfaces;

namespace InspectionCrawler.Domain.Model
{
    public abstract class Inspector : IInspector
    {
        protected readonly IInspectorLog Log;

        protected Inspector(IInspectorLog log)
        {
            Log = log;
            IsErrorEnabled = log.LogLevel.IsErrorEnabled();
            IsInfoEnabled = log.LogLevel.IsInfoEnabled();
            IsWarningEnabled = log.LogLevel.IsWarningEnabled();
        }

        public abstract string Name { get; }
        public virtual void CrawlStarting() { }
        public virtual void CrawlCompleted() { }
        public abstract void InspectPage(Page page);

        protected bool IsErrorEnabled { get; }
        protected bool IsInfoEnabled { get; }
        protected bool IsWarningEnabled { get; }

        protected void Error(string message, Uri uri = null)
        {
            LogMessage(LogType.Error, message, null, uri);
        }

        protected void Error(string message, Exception exception, Uri uri = null)
        {
            LogMessage(LogType.Error, message, exception, uri);
        }

        protected void Info(string message, Uri uri = null)
        {
            LogMessage(LogType.Info, message, null, uri);
        }

        protected void Info(string message, Exception exception, Uri uri = null)
        {
            LogMessage(LogType.Info, message, exception, uri);
        }

        protected void Warning(string message, Uri uri = null)
        {
            LogMessage(LogType.Warning, message, null, uri);
        }

        protected void Warning(string message, Exception exception, Uri uri = null)
        {
            LogMessage(LogType.Warning, message, exception, uri);
        }

        private void LogMessage(LogType logType, string message, Exception exception = null, Uri uri = null)
        {
            Log.Log(new InspectorLogMessage(this, new LogMessage(logType, message, exception, uri)));
        }
    }
}
