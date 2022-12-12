
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class PrimitiveTypeParamCommandSpec : ICommandSpecification<int, int>
{
    public Task<int> ExecuteAsync(int param, CancellationToken token = default)
    {
        return Task.FromResult(param);
    }
}