
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class ThrowsCommandSpec : ICommandSpecification<Voidy?, Voidy?>
{
    public Task<Voidy?> ExecuteAsync(Voidy? voidyParameter, CancellationToken token = default)
    {
        throw new InvalidOperationException();
    }
}
