
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PlasticCommand.UnitTests.TestCommands;
using PlasticCommand.UnitTests.TestCommands.Helpers;
using Xunit;

namespace PlasticCommand.UnitTests;

public class PipelineContextTests
{
    [Fact]
    public async Task pass_a_given_parameter()
    {
        var services = new ServiceCollection();
        var pipeMock = new TestPipe();
        var pipelineBuilder = new BuildPipeline(_ =>
        {
            return new IPipe[]{ pipeMock };
        });

        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();
        TestCommand sut = provider.GetRequiredService<TestCommand>();
        int expectedParam = -55;

        // Act
        _ = await sut.ExecuteAsync(expectedParam).ConfigureAwait(false);

        // Assert
        pipeMock.Context.Parameter.Should().Be(expectedParam);
    }

    [Fact]
    public async Task pass_a_given_param_as_empty_if_given_param_is_void()
    {
        var services = new ServiceCollection();
        var pipeMock = new TestPipe();
        var pipelineBuilder = new BuildPipeline(_ =>
        {
            return new IPipe[] { pipeMock };
        });

        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();
        VoidParamCommand sut = provider.GetRequiredService<VoidParamCommand>();

        // Act
        _ = await sut.ExecuteAsync(default).ConfigureAwait(false);

        // Assert
        pipeMock.Context.Parameter.Should().BeNull();
    }

    [Fact]
    public async Task pass_service_instances_that_to_be_injected_into_the_command()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITest2Service, Test2Service>();

        var pipeSpy = new TestPipe();
        var pipelineBuilder = new BuildPipeline(_ =>
        {
            return new IPipe[] { pipeSpy };
        });

        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();
        ServiceCommand sut = provider.GetRequiredService<ServiceCommand>();

        // Act
        IEnumerable<object> servicesInjected
            = await sut.ExecuteAsync(default).ConfigureAwait(false);

        // Assert
        pipeSpy.Context
                    .Services.Should().BeEquivalentTo(servicesInjected, options =>
                    {
                        return options.WithoutStrictOrdering();
                    });
    }

    [Fact]
    public async Task pass_a_type_of_command_spec()
    {
        var services = new ServiceCollection();
        var pipeSpy = new TestPipe();
        var pipelineBuilder = new BuildPipeline(_ =>
        {
            return new IPipe[] { pipeSpy };
        });

        services.AddPlastic(pipelineBuilder);
        IServiceProvider provider = services.BuildServiceProvider();
        TestCommand sut = provider.GetRequiredService<TestCommand>();

        // Act
        _ = await sut.ExecuteAsync(default).ConfigureAwait(false);

        // Assert
        pipeSpy.Context.CommandSpec.Should().Be(typeof(TestCommandSpec));
    }

    private sealed class TestPipe : IPipe
    {
        public PipelineContext Context { get; private set; } = default!;

        public Task<object?> Handle(PipelineContext context, Behavior nextBehavior, CancellationToken token = default)
        {
            this.Context = context;

            return nextBehavior();
        }
    }
}
