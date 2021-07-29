namespace Plastic.UnitTests
{
    using System.Collections.Concurrent;
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

            var sut = new FakeCommand(getService);

            // Act
            Response response = sut.ExecuteAsync(new NoParameters()).Result;

            // Assert
            response.HasSucceed().Should().BeTrue();
        }

        [Fact]
        public void Generated_command_does_execute_command_through_pipeline()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            PlasticInitializer.AddGeneratedCommands((type) => serviceCollection.AddTransient(type));

            var logger = new ConcurrentQueue<int>();
            serviceCollection.AddTransient(_ => logger);

            var pipeline = new BuildPipeline(() => new IPipe[]
            {
                new FakePipe(logger, 1, 2),
                new FakePipe(logger, 3, 4),
                new FakePipe(logger, 5, 6)
            });
            serviceCollection.AddTransient(_ => pipeline);

            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            GetService getService = (service) => provider.GetService(service);
            var sut = new FakeCommand(getService);

            // Act
            Response response = sut.ExecuteAsync(new NoParameters()).Result;

            // Assert
            int[] expectedLog = new int[] { 1, 3, 5, -1, 6, 4, 2 };
            logger.Should().BeEquivalentTo(expectedLog);
        }

        public class FakePipe : IPipe
        {
            private readonly ConcurrentQueue<int> _mornitor;
            private readonly int _valueToWriteBefore;
            private readonly int _valueToWriteAfter;

            public FakePipe(ConcurrentQueue<int> mornitor, int valueToWriteBefore, int valueToWriteAfter)
            {
                this._mornitor = mornitor;
                this._valueToWriteBefore = valueToWriteBefore;
                this._valueToWriteAfter = valueToWriteAfter;
            }

            public async Task<Response> Handle(
                PipelineContext context, Behavior<Response> nextBehavior, CancellationToken token)
            {
                this._mornitor.Enqueue(this._valueToWriteBefore);

                Response response = await nextBehavior.Invoke();

                this._mornitor.Enqueue(this._valueToWriteAfter);

                return response;
            }
        }

        public class FakeCommandSpec : CommandSpecificationBase
        {
            private readonly ConcurrentQueue<int>? _logger;

            public FakeCommandSpec(ConcurrentQueue<int>? logger = null)
            {
                this._logger = logger;
            }

            public override Task<Response> CanExecuteAsync(NoParameters request, CancellationToken token = default)
            {
                this._logger?.Enqueue(-1);
                return RespondWithSuccess();
            }

            public override Task<Response> ExecuteAsync(NoParameters request, CancellationToken token = default)
            {
                this._logger?.Enqueue(-1);
                return RespondWithSuccess();
            }
        }
    }
}
