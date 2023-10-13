using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PlasticCommand.UnitTests.TestCommands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PlasticCommand.UnitTests;

public class PipelineTests
{
    [Fact]
    public async Task execute_pipeline_and_command_in_direction()
    {
        var logger = new Queue<int>();
        int[] expectedDirection = new int[] { 1, 2, 3, -1, 3, 2, 1 };
        BuildPipeline pipelineBuilder = (_) =>
        {
            return new IPipe[]
            {
                new RecordingPipe(1, logger),
                new RecordingPipe(2, logger),
                new RecordingPipe(3, logger),
            };
        };

        var services = new ServiceCollection();
        services.AddScoped(_ => logger);
        services.AddScoped<HashSet<int>>();
        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();

        RecordingCommand sut = provider.GetRequiredService<RecordingCommand>();

        _ = await sut.ExecuteAsync(-1).ConfigureAwait(false);

        logger.Should()
                .BeEquivalentTo(expectedDirection, options => options.WithStrictOrdering());
    }

    [Fact]
    public void stop_pipline_executing_if_command_throws_any_exception()
    {
        var logger = new Queue<int>();
        int[] direction = new int[] { 1, 2, 3 };
        BuildPipeline pipelineBuilder = (_) =>
        {
            return new IPipe[]
            {
                new RecordingPipe(1, logger),
                new RecordingPipe(2, logger),
                new RecordingPipe(3, logger),
            };
        };

        var services = new ServiceCollection();
        services.AddScoped(_ => logger);
        services.AddScoped<HashSet<int>>();
        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();

        ThrowsCommand sut = provider.GetRequiredService<ThrowsCommand>();

        Func<Task> throwsExecuting = () => sut.ExecuteAsync(default);

        throwsExecuting.Should().ThrowAsync<Exception>();
        logger.Should().BeEquivalentTo(direction, options => options.WithStrictOrdering());
    }

    private sealed class RecordingPipe : IPipe
    {
        private readonly int _pipelineOrder;
        private readonly Queue<int> _store;

        public RecordingPipe(int pipelineOrder, Queue<int> store)
        {
            this._pipelineOrder = pipelineOrder;
            this._store = store;
        }

        public async Task<object?> Handle(
            PipelineContext context, Behavior nextBehavior, CancellationToken token = default)
        {
            this._store.Enqueue(this._pipelineOrder);

            object? result = await nextBehavior.Invoke();

            this._store.Enqueue(this._pipelineOrder);

            return result;
        }
    }
}

public class RecordingCommandSpec : ICommandSpecification<int, Voidy?>
{
    private readonly Queue<int> _logger;

    public RecordingCommandSpec(Queue<int> logger)
    {
        this._logger = logger;
    }

    public Task<Voidy?> ExecuteAsync(int param, CancellationToken token = default)
    {
        this._logger.Enqueue(param);
        return Task.FromResult<Voidy?>(default);
    }
}
