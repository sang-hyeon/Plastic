namespace Plastic.UnitTests.Internal.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class InternalCommandSpecificationBaseTests
    {
        [Fact]
        public void ExecuteAsync_does_return_failure_if_CanExecuteAsync_is_failed()
        {
            // Arrange
            var sut = new TestCommandSpec(false);

            // Act
            ExecutionResult<int> result = sut.ExecuteAsync(0).Result;

            // Assert
            result.HasSucceeded().Should().BeFalse();
        }

        [Fact]
        public void ExecuteAsync_does_return_success_if_CanExecuteAsync_is_success()
        {
            // Arrange
            var sut = new TestCommandSpec(true);

            // Act
            ExecutionResult<int> result = sut.ExecuteAsync(0).Result;

            // Assert
            result.HasSucceeded().Should().BeTrue();
        }

        private class TestCommandSpec : InternalCommandSpecificationBase<int, ExecutionResult<int>>
        {
            private readonly bool _canBeExecuted;

            public TestCommandSpec(bool canBeExecuted = false)
            {
                this._canBeExecuted = canBeExecuted;
            }

            public override Task<Response> CanExecuteAsync(int param, CancellationToken token = default)
            {
                return this._canBeExecuted ? CanBeExecutedTask() : CannotBeExecutedTask(string.Empty);
            }

            protected override Task<ExecutionResult<int>> OnExecuteAsync(int param, CancellationToken token = default)
            {
                return SuccessTask(param);
            }

            protected override ExecutionResult<int> CreateFailure(string? message)
            {
                return Failure<int>(message);
            }
        }
    }
}
