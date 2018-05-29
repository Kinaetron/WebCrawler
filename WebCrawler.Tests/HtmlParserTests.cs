using FluentAssertions;
using System;
using Xunit;

namespace WebCrawler.Tests
{
    public class HtmlParserTests
    {
        [Fact]
        public void Ctor_WithNullUri_ShouldThrowException()
        {
            var error = Record.Exception(() => new HtmlParser(null));

            error.Should().BeOfType<ArgumentNullException>();
            error.Message.Should().Be("Value cannot be null.\r\nParameter name: url");
        }
    }
}
