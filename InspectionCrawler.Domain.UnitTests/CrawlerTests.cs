using System;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace InspectionCrawler.Domain.UnitTests
{
    public class CrawlerTests
    {
        [Fact]
        public void CallFlushOnce()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var logHandler = fixture.Freeze<Mock<ILogHandler>>();
            var crawler = fixture.Create<Crawler>();

            // Act
            crawler.Crawl(new Uri("http://www.google.com/"));

            // Assert
            logHandler.Verify(x => x.Flush(), Times.Once);
        }

        [Fact]
        public void CallCrawlOnce()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var crawl = fixture.Freeze<Mock<ICrawl>>();
            var crawler = fixture.Create<Crawler>();
            var uri = new Uri("http://www.google.com/");

            // Act
            crawler.Crawl(uri);

            // Assert
            crawl.Verify(x => x.Crawl(uri, It.IsAny<Action<Page>>()), Times.Once);
        }
    }
}
