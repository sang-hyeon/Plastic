
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class ThrowsCommandSpec : ICommandSpecification<Voidy?, Voidy?>
{
    /// <summary>
    /// </summary>
    /// <param name="voidyParameter"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<Voidy?> ExecuteAsync(Voidy? voidyParameter, CancellationToken token = default)
    {
        throw new InvalidOperationException();
    }
}
