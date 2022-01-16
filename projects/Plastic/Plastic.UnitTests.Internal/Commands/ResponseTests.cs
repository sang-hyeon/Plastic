namespace Plastic.UnitTests.Internal.Commands
{
    using FluentAssertions;
    using Xunit;

    public class ResponseTests
    {
        [Theory]
        [InlineData(true, default(string))]
        [InlineData(false, default(string))]
        [InlineData(true, "")]
        [InlineData(false, "")]
        [InlineData(true, "Seotaiji")]
        [InlineData(false, "Damien rice")]
        public void Constructor_does_construct_Response(
            bool expectedSuccess, string? expectedMessage)
        {
            // Arrange
            // Act
            var sut = new Response(expectedSuccess, expectedMessage);

            // Assert
            sut.Result.Should().Be(expectedSuccess);
            sut.Message.Should().Be(expectedMessage);
        }
    }
}
