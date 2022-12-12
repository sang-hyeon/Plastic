using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public class TestValidationCommandSpec
    : ICommandSpecificationWithValidation<int, int, bool>
{
    public Task<bool> CanExecuteAsync(int param, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }

    public Task<int> ExecuteAsync(int param, CancellationToken token = default)
    {
        return Task.FromResult(param);
    }
}
