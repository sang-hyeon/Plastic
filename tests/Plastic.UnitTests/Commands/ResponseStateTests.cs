namespace Plastic.UnitTests
{
    using FluentAssertions;
    using Xunit;

    public class ResponseStateTests
    {
        [Theory]
        [InlineData(true, default(string))]
        [InlineData(false, default(string))]
        [InlineData(true, "")]
        [InlineData(false, "")]
        [InlineData(true, "Seotaiji")]
        [InlineData(false, "Damien rice")]
        public void Constructor_does_construct(
            bool expectedSuccess, string? expectedMessage)
        {
            // Arrange
            // Act
            var sut = new ResponseState(expectedSuccess, expectedMessage);

            // Assert
            sut.Success.Should().Be(expectedSuccess);
            sut.Message.Should().Be(expectedMessage);
        }
    }
}
