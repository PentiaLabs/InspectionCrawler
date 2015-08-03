using System;

namespace InspectionCrawler.Domain.Model
{
    public struct LogMessage : IEquatable<LogMessage>
    {
        public LogMessage(LogType logType, string message) : this()
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            LogType = logType;
            Message = message;
        }

        public LogMessage(LogType logType, string message, Uri uri) : this(logType, message, null, uri) { }

        public LogMessage(LogType logType, string message, Exception exception) : this(logType, message, exception, null) { }

        public LogMessage(LogType logType, string message, Exception exception, Uri uri) : this(logType, message)
        {
            Exception = exception;
            Uri = uri;
        }

        public LogType LogType { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public Uri Uri { get; }

        public override int GetHashCode()
        {
            var hashCode = LogType.GetHashCode() ^ Message.GetHashCode();
            if(Exception != null) hashCode ^= Exception.GetHashCode();
            if(Uri != null) hashCode ^= Uri.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is LogMessage)
                return Equals((LogMessage)obj);
            return false;
        }

        public bool Equals(LogMessage other)
        {
            return LogType == other.LogType && Message == other.Message && Exception == other.Exception && Uri == other.Uri;
        }

        public static bool operator ==(LogMessage message1, LogMessage message2)
        {
            return message1.Equals(message2);
        }

        public static bool operator !=(LogMessage message1, LogMessage message2)
        {
            return !message1.Equals(message2);
        }
    }
}
