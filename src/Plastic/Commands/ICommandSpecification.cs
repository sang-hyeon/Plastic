namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICommandSpecification<in TParam, TResult>
        where TResult : ExecutionResult
    {
        Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default);

        Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);
    }
}
