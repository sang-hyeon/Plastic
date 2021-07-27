namespace Plastic.UnitTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Plastic;
    using Plastic.Commands;
    using Xunit;

    public class GeneratedCommandTests
    {
        [Fact]
        public void Generated_command_does_execute_command()
        {
            // Arrange
            var sut = new TestCommand(FakeGetService);

            // Act
            Response response = sut.ExecuteAsync(new NoParameters()).Result;

            // Assert
            response.HasSucceed().Should().BeTrue();
        }

        public static object? FakeGetService(Type type)
        {
            if (type == typeof(TestCommandSpec))
                return new TestCommandSpec();
            else
                return default;
        }


        public class TestCommandSpec : CommandSpecificationBase
        {
            public TestCommandSpec()
            {
            }

            public override Task<Response> CanExecuteAsync(NoParameters request, CancellationToken token = default)
            {
                return RespondWithSuccess();
            }

            public override Task<Response> ExecuteAsync(NoParameters request, CancellationToken token = default)
            {
                return RespondWithSuccess();
            }
        }
    }
}
