namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class ParameterlessCommandSpecificationBase
        : CommandSpecInnerBase, ICommandSpecification<NoParameters, ExecutionResult>
    {
        public abstract Task<Response> CanExecuteAsync(NoParameters param, CancellationToken token = default);

        public abstract Task<ExecutionResult> ExecuteAsync(NoParameters param, CancellationToken token = default);
    }

    public abstract class ParameterlessCommandSpecificationBase<TResult>
        : CommandSpecInnerBase, ICommandSpecification<NoParameters, ExecutionResult<TResult>>
    {
        public abstract Task<Response> CanExecuteAsync(NoParameters param, CancellationToken token = default);

        public abstract Task<ExecutionResult<TResult>> ExecuteAsync(NoParameters param, CancellationToken token = default);
    }

    public abstract class CommandSpecificationBase<TParam, TResult>
        : CommandSpecInnerBase, ICommandSpecification<TParam, ExecutionResult<TResult>>
    {
        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        public abstract Task<ExecutionResult<TResult>> ExecuteAsync(TParam param, CancellationToken token = default);
    }

    public abstract class CommandSpecificationBase<TParam>
        : CommandSpecInnerBase, ICommandSpecification<TParam, ExecutionResult>
    {
        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        public abstract Task<ExecutionResult> ExecuteAsync(TParam param, CancellationToken token = default);
    }
}
