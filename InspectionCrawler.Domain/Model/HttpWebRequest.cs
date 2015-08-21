using System;
using System.Net;

namespace InspectionCrawler.Domain.Model
{
    public sealed class HttpWebRequest
    {
        public HttpWebRequest(
            string accept, 
            Uri address, 
            string connection, 
            long contentLength, 
            string contentType, 
            CookieContainer cookieContainer, 
            string expect,
            WebHeaderCollection headers, 
            string host, 
            string mediaType, 
            string method, 
            Version protocolVersion,
            Uri requestUri, 
            string userAgent)
        {
            Accept = accept;
            Address = address;
            Connection = connection;
            ContentLength = contentLength;
            ContentType = contentType;
            CookieContainer = cookieContainer;
            Expect = expect;
            Headers = headers;
            Host = host;
            MediaType = mediaType;
            Method = method;
            ProtocolVersion = protocolVersion;
            RequestUri = requestUri;
            UserAgent = userAgent;
        }

        public string Accept { get; }
        public Uri Address { get; }
        public string Connection { get; }
        public long ContentLength { get; }
        public string ContentType { get; }
        public CookieContainer CookieContainer { get; }
        public string Expect { get; }
        public WebHeaderCollection Headers { get; }
        public string Host { get; }
        public string MediaType { get; }
        public string Method { get; }
        public Version ProtocolVersion { get; }
        public Uri RequestUri { get; }
        public string UserAgent { get; }
    }
}
