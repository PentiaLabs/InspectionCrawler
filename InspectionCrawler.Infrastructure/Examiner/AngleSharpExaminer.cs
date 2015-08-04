using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using InspectionCrawler.Infrastructure.Extensions;

namespace InspectionCrawler.Infrastructure.Examiner
{
    public class AngleSharpExaminer : IExaminer
    {
        private static readonly HashSet<string> ValidSchemes;

        private readonly IDocument _document;
        private readonly HashSet<Uri> _links;

        static AngleSharpExaminer()
        {
            ValidSchemes = new HashSet<string>(
                new[]
                {
                    Uri.UriSchemeFile,
                    Uri.UriSchemeFtp,
                    Uri.UriSchemeGopher,
                    Uri.UriSchemeHttp,
                    Uri.UriSchemeHttps,
                    Uri.UriSchemeMailto,
                    Uri.UriSchemeNetPipe,
                    Uri.UriSchemeNews,
                    Uri.UriSchemeNetTcp,
                    Uri.UriSchemeNntp
                }
            );
        }

        public AngleSharpExaminer(ILog log, Uri uri, string content)
        {
            var parser = new HtmlParser();
            _document = parser.Parse(content);
            _links = new HashSet<Uri>();

            var baseUri = new Uri(uri.Scheme + Uri.SchemeDelimiter + uri.Host);

            foreach (var link in _document.Links)
            {
                var href = link.GetAttribute("href");
                if (string.IsNullOrWhiteSpace(href))
                {
                    log.Log(new LogMessage(LogType.Error, "A-tag has missing or empty href", uri));
                    continue;
                }

                href = href.Trim();

                if (href[0] == '#') continue;

                var schemeDelimiterPosition = href.IndexOf(':');
                if (schemeDelimiterPosition != -1)
                {
                    if (!ValidSchemes.Contains(href.Substring(0, schemeDelimiterPosition)))
                    {
                        if(log.LogLevel.IsWarningEnabled())
                            log.Log(new LogMessage(LogType.Warning, $"A-tag has unknown scheme ({href})", uri));

                        continue;
                    }
                }

                if (!Uri.IsWellFormedUriString(href, UriKind.RelativeOrAbsolute))
                {
                    log.Log(new LogMessage(LogType.Error, $"A-tag has invalid href ({href})", uri));
                    return;
                }

                try
                {
                    _links.Add(new Uri(baseUri, href));
                }
                catch (UriFormatException exception)
                {
                    log.Log(new LogMessage(LogType.Error, $"A-tag has invalid href ({href})", exception, uri));
                }
            }
        }

        public string GetText(string query)
        {
            var element = _document.QuerySelector(query);
            return element?.TextContent;
        }

        public string GetAttribute(string query, string name)
        {
            var element = _document.QuerySelector(query);
            return element?.GetAttribute(name);
        }

        public IEnumerable<Uri> Links => _links;
    }
}
