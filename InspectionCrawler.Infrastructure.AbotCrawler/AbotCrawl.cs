﻿using System;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using Abot.Crawler;
using Abot.Poco;

namespace InspectionCrawler.Infrastructure.AbotCrawler
{
    public class AbotCrawl : ICrawl
    {
        private readonly ILog _log;

        public AbotCrawl(ILog log)
        {
            _log = log;
        }

        public void Crawl(Uri uri, Action<Page> callback)
        {
            var crawlConfig = new CrawlConfiguration
            {
                CrawlTimeoutSeconds = 0,
                MaxConcurrentThreads = 5,
                UserAgentString = "InspectionCrawler v1.0",
                MinCrawlDelayPerDomainMilliSeconds = 1000,
                MaxPagesToCrawl = 0,
                MaxPagesToCrawlPerDomain = 0,
                MaxCrawlDepth = int.MaxValue
            };

            var crawler = new PoliteWebCrawler(crawlConfig);

            crawler.PageCrawlCompletedAsync += (sender, args) =>
            {
                var page = args.CrawledPage;

                if (page.WebException != null && page.HttpWebResponse == null)
                {
                    _log.Log(new LogMessage(LogType.Error, "Could not get page", page.WebException, page.Uri));
                    return;
                }

                callback(Convert(args.CrawledPage));

            };

            crawler.Crawl(uri);
        }

        private Page Convert(CrawledPage crawledPage)
        {
            var requestCompleted = crawledPage.RequestCompleted;
            if (crawledPage.DownloadContentCompleted.HasValue)
                requestCompleted = crawledPage.DownloadContentCompleted.Value;

            var byteSize = 0;
            if (crawledPage.Content.Bytes != null)
                byteSize = crawledPage.Content.Bytes.Length;

            var page = new Page(
                crawledPage.Uri,
                crawledPage.ParentUri,
                crawledPage.RequestStarted.ToUniversalTime(),
                requestCompleted.ToUniversalTime(),
                Convert(crawledPage.HttpWebRequest),
                Convert(crawledPage.HttpWebResponse),
                crawledPage.Content.Text,
                byteSize);

            return page;
        }

        private HttpWebRequest Convert(System.Net.HttpWebRequest request)
        {
            return new HttpWebRequest(
                request.Accept,
                request.Address,
                request.Connection,
                request.ContentLength,
                request.ContentType,
                request.CookieContainer,
                request.Expect,
                request.Headers,
                request.Host,
                request.MediaType,
                request.Method,
                request.ProtocolVersion,
                request.RequestUri,
                request.UserAgent);
        }

        private HttpWebResponse Convert(HttpWebResponseWrapper wrapper)
        {
            return new HttpWebResponse(
                wrapper.StatusCode,
                wrapper.ContentType,
                wrapper.ContentLength,
                wrapper.Headers,
                wrapper.CharacterSet,
                wrapper.ContentEncoding,
                wrapper.Cookies,
                wrapper.IsFromCache,
                wrapper.IsMutuallyAuthenticated,
                wrapper.LastModified,
                wrapper.Method,
                wrapper.ProtocolVersion,
                wrapper.ResponseUri,
                wrapper.Server,
                wrapper.StatusDescription);
        }
    }
}