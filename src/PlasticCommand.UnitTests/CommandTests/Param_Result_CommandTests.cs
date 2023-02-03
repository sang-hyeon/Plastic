
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PlasticCommand.UnitTests.TestCommands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PlasticCommand.UnitTests.CommandTests;

public class Param_Result_CommandTests
{
    [Fact]
    public void generate_a_command_with_primitive_type_param()
    {
        IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

        Action generating = () => _ = new PrimitiveTypeParamCommand(serviceProvider);

        generating.Should().NotThrow();
    }

    [Fact]
    public void generate_a_command_with_reference_type_param()
    {
        IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

        Action generating = () => _ = new ReferenceTypeParamCommand(serviceProvider);

        generating.Should().NotThrow();
    }

    [Fact]
    public async Task execute_a_commandAsync()
    {
        var services = new ServiceCollection();
        services.AddPlastic();
        ServiceProvider provider = services.BuildServiceProvider();

        var sut = new PrimitiveTypeParamCommand(provider);

        var result = await sut.ExecuteAsync(1);

        result.Should().Be(1);
    }

    [Fact]
    public void generate_interface_of_command()
    {
    }
}
