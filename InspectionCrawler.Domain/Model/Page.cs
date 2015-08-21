using System;
using InspectionCrawler.Domain.Interfaces;

namespace InspectionCrawler.Domain.Model
{
    public sealed class Page
    {
        public Page(
            Uri uri, 
            Uri referrer, 
            DateTime requestStartedOnUtc, 
            DateTime requestCompletedOnUtc,
            HttpWebRequest request, 
            HttpWebResponse response, 
            string content, 
            int contentByteSize)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (referrer == null) throw new ArgumentNullException(nameof(referrer));
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (content == null) throw new ArgumentNullException(nameof(content));

            Uri = uri;
            Referrer = referrer;
            RequestStartedOnUtc = requestStartedOnUtc;
            RequestCompletedOnUtc = requestCompletedOnUtc;
            Request = request;
            Response = response;
            Content = content;
            ContentByteSize = contentByteSize;
        }

        public Uri Uri { get; }
        public Uri Referrer { get; }
        public DateTime RequestStartedOnUtc { get; }
        public DateTime RequestCompletedOnUtc { get; }
        public HttpWebRequest Request { get; }
        public HttpWebResponse Response { get; }
        public string Content { get; }
        public int ContentByteSize { get; }
        public IExaminer Examiner { get; internal set; }
    }
}
