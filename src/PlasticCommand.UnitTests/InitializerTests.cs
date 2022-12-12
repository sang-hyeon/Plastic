using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PlasticCommand.UnitTests.TestCommands;
using Xunit;

namespace PlasticCommand.UnitTests;

public class InitializerTests
{
    [Fact]
    public void register_command_spec()
    {
        var services = new ServiceCollection();
        services.AddPlastic();
        ServiceProvider provider = services.BuildServiceProvider();

        TestCommandSpec? actualCommandSpec = provider.GetService<TestCommandSpec>();

        actualCommandSpec.Should().NotBeNull();
    }

    [Fact]
    public void register_generated_command()
    {
        var services = new ServiceCollection();
        services.AddPlastic();
        ServiceProvider provider = services.BuildServiceProvider();

        TestCommand? actualCommand = provider.GetService<TestCommand>();

        actualCommand.Should().NotBeNull();
    }
}
