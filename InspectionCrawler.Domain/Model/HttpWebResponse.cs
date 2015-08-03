using System;
using System.Collections.Specialized;
using System.Net;

namespace InspectionCrawler.Domain.Model
{
    public sealed class HttpWebResponse
    {
        public HttpWebResponse(
            HttpStatusCode statusCode, 
            string contentType,
            long contentLength, 
            NameValueCollection headers, 
            string characterSet, 
            string contentEncoding, 
            CookieCollection cookies, 
            bool isFromCache,
            bool isMutuallyAuthenticated,
            DateTime lastModified,
            string method,
            Version protocolVersion,
            Uri responseUri,
            string server,
            string statusDescription)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            ContentLength = contentLength;
            Headers = headers;
            CharacterSet = characterSet;
            ContentEncoding = contentEncoding;
            Cookies = cookies;
            IsFromCache = isFromCache;
            IsMutuallyAuthenticated = isMutuallyAuthenticated;
            LastModified = lastModified;
            Method = method;
            ProtocolVersion = protocolVersion;
            ResponseUri = responseUri;
            Server = server;
            StatusDescription = statusDescription;
        }

        public HttpStatusCode StatusCode { get; }
        public string ContentType { get; }
        public long ContentLength { get; }
        public NameValueCollection Headers { get; }
        public string CharacterSet { get; }
        public string ContentEncoding { get; }
        public CookieCollection Cookies { get; }
        public bool IsFromCache { get; }
        public bool IsMutuallyAuthenticated { get; }
        public DateTime LastModified { get; }
        public string Method { get; }
        public Version ProtocolVersion { get; }
        public Uri ResponseUri { get; }
        public string Server { get; }
        public string StatusDescription { get; }
    }
}
