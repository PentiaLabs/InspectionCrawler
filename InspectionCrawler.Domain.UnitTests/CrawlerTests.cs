using System;
using System.Net;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;
using HttpWebRequest = InspectionCrawler.Domain.Model.HttpWebRequest;
using HttpWebResponse = InspectionCrawler.Domain.Model.HttpWebResponse;

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
        public void CallFlushOnLogHandlerOnce()
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
        public void CallCrawlOnLogHanlerOnce()
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
        public void CallCrawlStartingOnInspectorOnce()
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
        public void CallCrawlCompletedOnInspectorOnce()
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
        public void LogWhenInspectorThrowsExceptionAtCrawlStarting()
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
        public void LogWhenInspectorThrowsExceptionAtCrawlCompleted()
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

        [Fact]
        public void LogWhenInspectorThrowExceptionAtInspectPage()
        {
            // Arrange
            var page = CreatePage();
            var exception = new Exception("Test exception");
            var inspector = new Mock<IInspector>();
            var crawl = _fixture.Freeze<Mock<ICrawl>>();
            var logHandler = new Mock<ILogHandler>();
            crawl.Setup(c => c.Crawl(_uri, It.IsAny<Action<Page>>())).Callback<Uri, Action<Page>>((uri, action) => action(page));
            inspector.Setup(i => i.InspectPage(page)).Throws(exception);
            var crawler = new Crawler(crawl.Object, logHandler.Object, new Mock<IExaminerFactory>().Object, new[] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            logHandler.Verify(x => x.Log(It.Is<InspectorLogMessage>(
                l => l.Inspector.Name == inspector.Object.Name &&
                l.LogMessage.LogType == LogType.Error &&
                l.LogMessage.Exception == exception)), Times.Once);
        }

        [Fact]
        public void PassPageToInspectorOnce()
        {
            // Arrange
            var page = CreatePage();
            var inspector = new Mock<IInspector>();
            var crawl = _fixture.Freeze<Mock<ICrawl>>();
            crawl.Setup(c => c.Crawl(_uri, It.IsAny<Action<Page>>())).Callback<Uri, Action<Page>>((uri, action) => action(page));
            var crawler = new Crawler(crawl.Object, new Mock<ILogHandler>().Object, new Mock<IExaminerFactory>().Object, new[] { inspector.Object });

            // Act
            crawler.Crawl(_uri);

            // Assert
            inspector.Verify(i => i.InspectPage(page), Times.Once);
        }

        private Page CreatePage()
        {
            return new Page(
                _uri,
                _uri,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new HttpWebRequest("", _uri, "", 0, "", null, "", null, "", "", "", new Version(1, 1), _uri, ""),
                new HttpWebResponse(HttpStatusCode.OK, "", 0, null, "", "", null, false, false, DateTime.UtcNow, "", new Version(1, 1), _uri, "", ""),
                "", 
                0);
        }
    }
}
