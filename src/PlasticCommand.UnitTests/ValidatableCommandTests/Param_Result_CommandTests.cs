
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PlasticCommand.UnitTests.TestCommands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PlasticCommand.UnitTests.CommandTests;

public class Param_Result_ValidatableCommandTests
{
    [Fact]
    public void generate_a_command_with_primitive_type_param()
    {
        IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

        Action generating = () => _ = new TestValidationCommand(serviceProvider);

        generating.Should().NotThrow();
    }

    [Fact]
    public async Task can_execute()
    {
        var services = new ServiceCollection();
        services.AddPlastic();
        IServiceProvider provider = services.BuildServiceProvider();
        TestValidationCommand sut = provider.GetRequiredService<TestValidationCommand>();

        bool result = await sut.CanExecuteAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task execute_a_commandAsync()
    {
        var services = new ServiceCollection();
        services.AddPlastic();
        ServiceProvider provider = services.BuildServiceProvider();
        var sut = new TestValidationCommand(provider);

        var result = await sut.ExecuteAsync(1);

        result.Should().Be(1);
    }
}
