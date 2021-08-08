namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICommandSpecification<in TParam, TResult>
        where TParam : CommandParameters
        where TResult : ExecutionResult
    {
        Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default);

        Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);
    }

    public interface ICommandSpecification<TResult>
        : ICommandSpecification<NoParameters, TResult>
        where TResult : ExecutionResult
    {
    }

    public interface ICommandSpecification
        : ICommandSpecification<NoParameters, ExecutionResult>
    {
    }
}
