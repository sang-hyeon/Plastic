
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class VoidParamCommandSpec : ICommandSpecification<Voidy?, int>
{
    public Task<int> ExecuteAsync(Voidy? param, CancellationToken token = default)
    {
        return Task.FromResult(1);
    }
}
