namespace Plastic.UnitTests.Pipeline
{
    using System;
    using FluentAssertions;
    using Plastic;
    using Xunit;

    public class PipelineContextTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("ABC")]
        public void CreateFailure_does_return_ExecutionResult_failed(string? expectedMessage)
        {
            // Arrange
            var sut = new PipelineContext<int>(default, typeof(int), Array.Empty<string>());

            // Act
            ExecutionResult result = sut.CreateFailure(expectedMessage);

            // Assert
            result.Result.Should().BeFalse();
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void CreateFailure_does_return_ExecutionResult_typed()
        {
            // Arrange
            var sut = new PipelineContext<int>(default, typeof(int), Array.Empty<string>());

            // Act
            ExecutionResult result = sut.CreateFailure();

            // Assert
            result.Should().BeOfType<ExecutionResult<int>>();
        }
    }
}
