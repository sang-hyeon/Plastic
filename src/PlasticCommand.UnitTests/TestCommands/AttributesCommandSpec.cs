using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

[PlasticCommand(GeneratedCommandName = "AAACommand", GroupName = "AGroupCommands")]
public class AttributesCommandSpec : ICommandSpecification<int, int>
{
    public Task<int> ExecuteAsync(int param, CancellationToken token = default)
    {
        throw new System.NotImplementedException();
    }
}
