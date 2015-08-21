using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using InspectionCrawler.Domain.Extensions;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspectors
{
    public class ErrorDetector : Inspector
    {
        private readonly ConcurrentDictionary<Uri, HttpStatusCode> _checkedExternalLinks;
        private readonly bool _checkExternalLinks;
        private readonly bool _treatRedirectsAsErrors;

        public ErrorDetector(IInspectorLog log, bool checkExternalLinks, bool treatRedirectsAsErrors) : base(log)
        {
            if (checkExternalLinks)
                _checkedExternalLinks = new ConcurrentDictionary<Uri, HttpStatusCode>();

            _checkExternalLinks = checkExternalLinks;
            _treatRedirectsAsErrors = treatRedirectsAsErrors;
        }

        public override string Name => "Error detector";

        public override void InspectPage(Page page)
        {
            if (page.Response.StatusCode == HttpStatusCode.OK)
            {
                if (_checkExternalLinks)
                    CheckExternalLinks(page);

                return;
            }

            Error(page.Uri, page.Referrer, page.Response.StatusCode);
        }

        private void CheckExternalLinks(Page page)
        {
            var externalLinks = page.GetExternalLinks();
            foreach (var externalLink in externalLinks)
            {
                try
                {
                    HttpStatusCode httpStatusCode;
                    if (_checkedExternalLinks.ContainsKey(externalLink))
                        httpStatusCode = _checkedExternalLinks[externalLink];
                    else
                    {
                        httpStatusCode = GetHttpStatusCode(externalLink);
                        _checkedExternalLinks.GetOrAdd(externalLink, httpStatusCode);
                    }

                    if (httpStatusCode != HttpStatusCode.OK)
                        Error(externalLink, page.Uri, httpStatusCode);
                }
                catch (Exception exception)
                {
                    Error("Could not get page", exception, externalLink);
                }
            }
        }

        private void Error(Uri uri, Uri referrer, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.MovedPermanently ||
                statusCode == HttpStatusCode.Redirect ||
                statusCode == HttpStatusCode.RedirectMethod ||
                statusCode == HttpStatusCode.RedirectKeepVerb ||
                statusCode == HttpStatusCode.TemporaryRedirect)
            {
                if (!_treatRedirectsAsErrors) return;
            }

            var code = (int)statusCode;
            Error($"{uri} (referrer: {referrer}) returned {code}");
        }

        public override void CrawlStarting() { }
        public override void CrawlCompleted() { }

        private HttpStatusCode GetHttpStatusCode(Uri uri)
        {
            using (var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false }))
            {
                return client.GetAsync(uri).Result.StatusCode;
            }
        }
    }
}
