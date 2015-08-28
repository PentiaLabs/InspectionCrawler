using System;
using System.Linq;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;
using Moq;
using Xunit;

namespace InspectionCrawler.Infrastructure.AngleSharpExamine.UnitTests
{
    public class AngleSharpExaminerTests
    {
        [Theory]
        [InlineData("<a href=''></a>")]
        [InlineData("<a href=' '></a>")]
        public void LogWhenHrefIsEmptyOrWhitespace(string content)
        {
            // Arrange
            var log = new Mock<ILog>();
            var uri = new Uri("http://google.com/");

            // Act
            var examiner = new AngleSharpExaminer(log.Object, uri, content);

            // Assert
            log.Verify(x => x.Log(It.Is<LogMessage>( l => l.LogType == LogType.Error && l.Uri == uri )), Times.Once);
        }

        [Theory]
        [InlineData("<a href='http://'></a>")]
        [InlineData("<a href='http:\\www.google.com/'></a>")]
        public void LogInvalidHrefs(string content)
        {
            // Arrange
            var log = new Mock<ILog>();
            var uri = new Uri("http://google.com/");

            // Act
            var examiner = new AngleSharpExaminer(log.Object, uri, content);

            // Assert
            log.Verify(x => x.Log(It.Is<LogMessage>(l => l.LogType == LogType.Error && l.Uri == uri)), Times.Once);
        }

        [Theory]
        [InlineData("<a href='#'></a>")]
        [InlineData("<a href='#SomeTag'></a>")]
        public void IgnoreAnchorTags(string content)
        {
            // Arrange
            var uri = new Uri("http://google.com/");

            // Act
            var examiner = new AngleSharpExaminer(new Mock<ILog>().Object, uri, content);

            // Assert
            Assert.Empty(examiner.Links);
        }

        [Fact]
        public void OnlyReturnSameUriOnce()
        {
            // Arrange
            var uri = new Uri("http://google.com/");
            var content = "<a href='Http://www.google.com/'></a><a href='Http://www.google.com/'></a>";

            // Act
            var examiner = new AngleSharpExaminer(new Mock<ILog>().Object, uri, content);

            // Assert
            Assert.Single(examiner.Links);
        }

        [Theory]
        [InlineData("<a href='somePage'></a>")]
        [InlineData("<a href='/somePage'></a>")]
        [InlineData("<a href='~/somePage'></a>")]
        public void PrefixRelativeUrlsWithHost(string content)
        {
            // Arrange
            var uri = new Uri("http://google.com/");

            // Act
            var examiner = new AngleSharpExaminer(new Mock<ILog>().Object, uri, content);

            // Assert
            Assert.Single(examiner.Links);
            Assert.Equal(uri.Host, examiner.Links.First().Host);
        }

        [Theory]
        [InlineData("<a href='http://www.google.com'></a>")]
        [InlineData("<a href='http://www.google.com/somePage'></a>")]
        [InlineData("<a href='http://www.google.com/somePage?with=queryString'></a>")]
        public void ReturnValidAbsoluteUrls(string content)
        {
            // Arrange
            var uri = new Uri("http://google.com/");

            // Act
            var examiner = new AngleSharpExaminer(new Mock<ILog>().Object, uri, content);

            // Assert
            Assert.Single(examiner.Links);
        }
    }
}
