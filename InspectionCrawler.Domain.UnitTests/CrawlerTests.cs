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
        private readonly IFixture _fixture;
        private readonly Uri _uri;

        public CrawlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _uri = new Uri("http://www.google.com/");
        }

        [Fact]
        public void CallFlushOnce()
        {
            // Arrange
            var logHandler = _fixture.Freeze<Mock<ILogHandler>>();
            var crawler = _fixture.Create<Crawler>();

            // Act
            crawler.Crawl(_uri);

            // Assert
            logHandler.Verify(x => x.Flush(), Times.Once);
        }

        [Fact]
        public void CallCrawlOnce()
        {
            // Arrange
            var crawl = _fixture.Freeze<Mock<ICrawl>>();
            var crawler = _fixture.Create<Crawler>();

            // Act
            crawler.Crawl(_uri);

            // Assert
            crawl.Verify(x => x.Crawl(_uri, It.IsAny<Action<Page>>()), Times.Once);
        }

        [Fact]
        public void CallCrawlStartingOnInspectorsOnce()
        {
            // Arrange
            var inspector = new Mock<IInspector>();
            var crawler = new Crawler(new Mock<ICrawl>().Object, new Mock<ILogHandler>().Object, new Mock<IExaminerFactory>().Object, new [] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            inspector.Verify(x => x.CrawlStarting(), Times.Once);
        }

        [Fact]
        public void CallCrawlCompletedOnInspectorsOnce()
        {
            // Arrange
            var inspector = new Mock<IInspector>();
            var crawler = new Crawler(new Mock<ICrawl>().Object, new Mock<ILogHandler>().Object, new Mock<IExaminerFactory>().Object, new[] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            inspector.Verify(x => x.CrawlCompleted(), Times.Once);
        }

        [Fact]
        public void LogWhenInspectorsThrowExceptionAtCrawlStarting()
        {
            // Arrange
            var inspector = new Mock<IInspector>();
            var exception = new Exception("Test exception");
            inspector.Setup(i => i.CrawlStarting()).Throws(exception);
            var logHandler = new Mock<ILogHandler>();
            var crawler = new Crawler(new Mock<ICrawl>().Object, logHandler.Object, new Mock<IExaminerFactory>().Object, new[] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            logHandler.Verify(x => x.Log(It.Is<InspectorLogMessage>(
                l => l.Inspector.Name == inspector.Object.Name && 
                l.LogMessage.LogType == LogType.Error && 
                l.LogMessage.Exception == exception)), Times.Once);
        }

        [Fact]
        public void LogWhenInspectorsThrowExceptionAtCrawlCompleted()
        {
            // Arrange
            var inspector = new Mock<IInspector>();
            var exception = new Exception("Test exception");
            inspector.Setup(i => i.CrawlCompleted()).Throws(exception);
            var logHandler = new Mock<ILogHandler>();
            var crawler = new Crawler(new Mock<ICrawl>().Object, logHandler.Object, new Mock<IExaminerFactory>().Object, new[] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            logHandler.Verify(x => x.Log(It.Is<InspectorLogMessage>(
                l => l.Inspector.Name == inspector.Object.Name && 
                l.LogMessage.LogType == LogType.Error && 
                l.LogMessage.Exception == exception)), Times.Once);
        }
    }
}
