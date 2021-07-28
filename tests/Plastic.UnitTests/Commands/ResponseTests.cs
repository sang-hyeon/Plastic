namespace Plastic.UnitTests
{
    using Xunit;
    using FluentAssertions;
    using AutoFixture.Xunit2;

    public class ResponseTests
    {
        [Theory]
        [InlineData(true, default(string))]
        [InlineData(false, default(string))]
        [InlineData(true, "")]
        [InlineData(false, "")]
        [InlineData(true, "Seotaiji")]
        [InlineData(false, "Damien rice")]
        [AutoData]
        public void Constructor_does_construct_with_given_parameters(
            bool expectedSuccess, string? expectedMessage)
        {
            // Arrange
            // Act
            var sut = new Response(expectedSuccess, expectedMessage);

            // Assert
            sut.State.Success.Should().Be(expectedSuccess);
            sut.State.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData(true, default(string))]
        [InlineData(false, default(string))]
        [InlineData(true, "")]
        [InlineData(false, "")]
        [InlineData(true, "Seotaiji")]
        [InlineData(false, "Damien rice")]
        [AutoData]
        public void Constructor_does_construct_with_given_ResponseState(
            bool expectedSuccess, string? expectedMessage)
        {
            // Arrange
            var expectedState = new ResponseState(expectedSuccess, expectedMessage);

            // Act
            var sut = new Response(expectedState);

            // Assert
            sut.State.Should().Be(expectedState);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HasSucceed_does_return_correctly(bool expectedSuccess)
        {
            // Arrange
            var sut = new Response(expectedSuccess, string.Empty);

            // Act
            bool succeed = sut.HasSucceed();

            // Assert
            succeed.Should().Be(expectedSuccess);
        }
    }
}
