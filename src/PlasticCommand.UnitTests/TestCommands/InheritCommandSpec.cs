using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand.UnitTests.TestCommands;

public abstract class InheritCommandSpecBase : ICommandSpecification<int, int>
{
    /// <inheritdoc cref="Execute"/>
    public Task<int> ExecuteAsync(int param, CancellationToken token = default)
    {
        return Execute();
    }

    protected abstract Task<int> Execute();
}

public class InheritCommandSpec : InheritCommandSpecBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="AbandonedMutexException"/>
    protected override Task<int> Execute()
    {
        return Task.FromResult(1);
    }
}
