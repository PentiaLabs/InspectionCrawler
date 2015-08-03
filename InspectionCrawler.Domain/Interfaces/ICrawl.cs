using System;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface ICrawl
    {
        void Crawl(Uri uri, Action<Page> callback);
    }
}
