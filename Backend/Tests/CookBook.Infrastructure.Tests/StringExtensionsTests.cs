using System;
using Shouldly;
using Xunit;

namespace CookBook.Infrastructure.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("Test Tag", "test-tag")]
        [InlineData(" Test Tag ", "test-tag")]
        [InlineData("Test-Tag", "test-tag")]
        [InlineData("Test's-Tag", "test-s-tag")]
        public void Canonicalize_HappyPath_Test(string input, string expected)
        {
            input.Canonicalize().ShouldBe(expected);
        }
    }
}