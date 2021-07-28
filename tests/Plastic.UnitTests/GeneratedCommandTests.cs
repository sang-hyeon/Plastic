namespace Plastic.UnitTests
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Plastic;
    using Plastic.Commands;
    using Xunit;

    public class GeneratedCommandTests
    {
        [Fact]
        public void Generated_command_does_execute_command_correctly()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            PlasticInitializer.AddGeneratedCommands((type) => serviceCollection.AddTransient(type));
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            GetService getService = (service) => provider.GetService(service);

            var sut = new TestCommand(getService);

            // Act
            Response response = sut.ExecuteAsync(new NoParameters()).Result;

            // Assert
            response.HasSucceed().Should().BeTrue();
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
