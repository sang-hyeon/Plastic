
using System.Threading;
using System.Threading.Tasks;

namespace PlasticCommand;

public interface ICommandSpecification<TParam, TResult>
{
    Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default);
}
