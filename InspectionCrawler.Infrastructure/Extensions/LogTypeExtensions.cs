using System;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Extensions
{
    public static class LogTypeExtensions
    {
        public static bool ShouldLog(this LogType logLevel, LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return logLevel.IsErrorEnabled();
                case LogType.Info:
                    return logLevel.IsInfoEnabled();
                case LogType.Warning:
                    return logLevel.IsWarningEnabled();
            }

            throw new ArgumentException($"Enum value is invalid ({logType})", nameof(logType));
        }

        public static bool IsErrorEnabled(this LogType logType)
        {
            return true;
        }

        public static bool IsInfoEnabled(this LogType logType)
        {
            return logType == LogType.Info;
        }

        public static bool IsWarningEnabled(this LogType logType)
        {
            return logType == LogType.Info || logType == LogType.Warning;
        }
    }
}
