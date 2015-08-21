using System;
using System.Collections.Generic;
using InspectionCrawler.Domain.Extensions;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspectors
{
    public class ExternalLinksCollector : Inspector
    {
        private readonly HashSet<Uri> _urisToIgnore;

        public ExternalLinksCollector(IInspectorLog log, IEnumerable<Uri> urisToIgnore) : base(log)
        {
            _urisToIgnore = new HashSet<Uri>(urisToIgnore);
        }

        public override string Name => "External links collector";

        public override void InspectPage(Page page)
        {
            var externalLinks = page.GetExternalLinks();
            foreach (var externalLink in externalLinks)
            {
                if(!_urisToIgnore.Contains(externalLink) && IsInfoEnabled)
                    Info($"{externalLink} found on {page.Uri}");
            }
        }
    }
}
