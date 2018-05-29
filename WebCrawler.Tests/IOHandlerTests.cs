using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;

namespace WebCrawler.Tests
{
    public class IOHandlerTests
    {
        private class Arrangement
        {
            public IOHandler SUT { get; }

            public Arrangement()
            {
                SUT = new IOHandler();
            }
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void EnterUrl_WithEmptyOrNullUrl_ShouldOutputMessageAndFalse(string input)
        {
            // Act
            var arrangement = new Arrangement();

            // Arrange 
           var result =  arrangement.SUT.ValidateUrl(input);

            // Assert
            result.Item1.Should().BeFalse();
            result.Item2.Should().Be("No url was passed");
        }

        [Fact]
        public void EnterUrl_WithInvalidUrl_ShouldOutputMessageAndFalse()
        {
            // Act
            var arrangement = new Arrangement();

            // Arrange 
            var result = arrangement.SUT.ValidateUrl("testing string");

            // Assert
            result.Item1.Should().BeFalse();
            result.Item2.Should().Be("The url isn't valid");
        }

        [Fact]
        public void EnterUrl_WithValidUrl_ShouldOutputTrue()
        {
            // Act
            var arrangement = new Arrangement();

            // Arrange 
            var result = arrangement.SUT.ValidateUrl("http://google.com");

            // Assert
            result.Item1.Should().BeTrue();
            result.Item2.Should().Be(string.Empty);
        }

        [Fact]
        public void ExportFile_NullCrawlResultModel_ShouldThrowException()
        {
            // Act
            var arrangement = new Arrangement();

            // Arrange 
            var error = Record.Exception(() => arrangement.SUT.ExportFile(null));

            // Assert
            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: result");
        }

        [Fact]
        public void ExportFile_WithCrawlResultsModel_ShouldSaveFile()
        {
            // Act
            var filename = "testFile";
            var arrangement = new Arrangement();

            var links = new List<string>();
            var models = new List<CrawlModel>();

            links.Add("testpage1.com");

            var model = new CrawlModel()
            {
                Page = "testpage.com",
                Links = links
            };

            models.Add(model);

            var crawlResults = new CrawlResultModel()
            {
                FileName = filename,
                CrawlModels = models
             };


            // Arrange
            arrangement.SUT.ExportFile(crawlResults);

            // Assert
            File.Exists(filename + ".json").Should().BeTrue();
        }
    }
}
