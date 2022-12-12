
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class ThrowsCommandSpec : ICommandSpecification<Voidy?, Voidy?>
{
    public Task<Voidy?> ExecuteAsync(Voidy? param, CancellationToken token = default)
    {
        throw new InvalidOperationException();
    }
}
