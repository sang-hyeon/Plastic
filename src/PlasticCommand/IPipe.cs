
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand;
public interface IPipe
{
    Task<object?> Handle(
        PipelineContext context, Behavior nextBehavior, CancellationToken token = default);
}

public delegate Task<object?> Behavior();
