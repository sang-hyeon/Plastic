
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class ReferenceTypeParamCommandSpec
    : ICommandSpecification<StringBuilder, StringBuilder>
{
    public Task<StringBuilder> ExecuteAsync(
        StringBuilder param, CancellationToken token = default)
    {
        return Task.FromResult(param);
    }
}
