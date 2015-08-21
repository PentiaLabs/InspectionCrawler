using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.AngleSharpExamine
{
    public class AngleSharpExaminer : IExaminer
    {
        private readonly IDocument _document;
        private readonly HashSet<Uri> _links;

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
