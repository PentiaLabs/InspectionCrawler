using System;
using System.Collections.Generic;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Extensions
{
    public static class PageExtensions
    {
        public static IEnumerable<Uri> GetExternalLinks(this Page page) 
        {
            var host = page.Uri.Host;

            var externalUris = new HashSet<Uri>();

            foreach (var link in page.Examiner.Links)
            {
                var scheme = link.Scheme;
                if (!(scheme.Equals(Uri.UriSchemeHttp) || scheme.Equals(Uri.UriSchemeHttps)))
                    continue;

                if (!link.Host.Equals(host))
                    externalUris.Add(link);
            }

            return externalUris;
        }
    }
}
