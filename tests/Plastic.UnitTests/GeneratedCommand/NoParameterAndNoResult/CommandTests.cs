namespace Plastic.UnitTests.GeneratedCommand.NoParameterAndNoResult
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Plastic;
    using Xunit;

    public class CommandTests
    {
        [Fact]
        public void ExecuteAsync_does_execute_command_correctly()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddPlastic();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var sut = new FakeCommand(provider);

            // Act
            ExecutionResult response = sut.ExecuteAsync(default).Result;

            // Assert
            response.HasSucceeded().Should().BeTrue();
        }

        [Fact]
        public void ExecuteAsync_does_execute_command_through_pipeline()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var logger = new ConcurrentQueue<int>();
            serviceCollection.AddTransient(_ => logger);

            var pipeline = new BuildPipeline(_ => new Pipe[]
            {
                new FakePipe(logger, 1, 2),
                new FakePipe(logger, 3, 4),
                new FakePipe(logger, 5, 6)
            });
            serviceCollection.AddPlastic(pipeline);
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var sut = new FakeCommand(provider);

            // Act
            ExecutionResult response = sut.ExecuteAsync(default).Result;

            // Assert
            int[] expectedLog = new int[] { 1, 3, 5, -1, 6, 4, 2 };
            logger.Should().BeEquivalentTo(expectedLog);
        }

        [Fact]
        public void ExecuteAsync_does_use_service_as_a_single_instance()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var logger = new ConcurrentQueue<int>();
            serviceCollection.AddScoped(_ => logger);

            var pipeline = new BuildPipeline(p => new Pipe[]
            {
                new FakePipe(p.GetRequiredService<ConcurrentQueue<int>>()),
                new FakePipe(p.GetRequiredService<ConcurrentQueue<int>>())
            });
            serviceCollection.AddPlastic(pipeline);
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var sut = new FakeCommand(provider);

            // Act
            ExecutionResult response = sut.ExecuteAsync(default).Result;

            // Assert
            logger.Should().HaveCount(5);
        }

        [Fact]
        public void ExecuteAsync_does_provide_PipelineContext_to_the_pipeline()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var spyService = new ConcurrentQueue<int>();
            serviceCollection.AddScoped(_ => spyService);

            FakePipe[]? pipeline = default;
            var pipelineBuilder = new BuildPipeline(p => pipeline = new FakePipe[]
            {
                new FakePipe(p.GetRequiredService<ConcurrentQueue<int>>()),
                new FakePipe(p.GetRequiredService<ConcurrentQueue<int>>())
            });

            serviceCollection.AddPlastic(pipelineBuilder);
            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            var sut = new FakeCommand(provider);

            // Act
            sut.ExecuteAsync(default).Wait();

            // Assert

            pipeline!.Select(q => q.ProvidedContext!.CommandSpec)
                        .Should()
                        .AllBeAssignableTo<FakeCommandSpec>();

            pipeline!.Select(q => q.ProvidedContext!.Parameter)
                        .Should()
                        .AllBeEquivalentTo(default(object?));

            pipeline!.SelectMany(q => q.ProvidedContext!.Services)
                        .All(q => q == spyService)
                        .Should()
                        .BeTrue();
        }
    }

    public class FakePipe : Pipe
    {
        private readonly ConcurrentQueue<int> _mornitor;
        private readonly int _valueToWriteBefore;
        private readonly int _valueToWriteAfter;

        public PipelineContext? ProvidedContext { get; private set; }

        public FakePipe(ConcurrentQueue<int> mornitor, int valueToWriteBefore = 0, int valueToWriteAfter = 0)
        {
            this._mornitor = mornitor;
            this._valueToWriteBefore = valueToWriteBefore;
            this._valueToWriteAfter = valueToWriteAfter;
        }

        public override async Task<ExecutionResult> Handle(
            PipelineContext context, Behavior<ExecutionResult> nextBehavior, CancellationToken token)
        {
            this.ProvidedContext = context;

            this._mornitor.Enqueue(this._valueToWriteBefore);

            ExecutionResult response = await nextBehavior.Invoke().ConfigureAwait(false);

            this._mornitor.Enqueue(this._valueToWriteAfter);

            return response;
        }
    }

    public class FakeCommandSpec : ParameterlessCommandSpecificationBase
    {
        private readonly ConcurrentQueue<int>? _logger;

        public FakeCommandSpec(ConcurrentQueue<int>? logger = null)
        {
            this._logger = logger;
        }

        public override Task<Response> CanExecuteAsync(NoParameters? param = default, CancellationToken token = default)
        {
            this._logger?.Enqueue(-1);
            return CanBeExecutedTask();
        }

        public override Task<ExecutionResult> ExecuteAsync(NoParameters? param = default, CancellationToken token = default)
        {
            this._logger?.Enqueue(-1);
            return SuccessTask();
        }
    }
}
