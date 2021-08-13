namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class ParameterlessCommandSpecificationBase
        : CanCreateResults, ICommandSpecification<NoParameters, ExecutionResult>
    {
        public abstract Task<Response> CanExecuteAsync(NoParameters param, CancellationToken token = default);

        public abstract Task<ExecutionResult> ExecuteAsync(NoParameters param, CancellationToken token = default);
    }

    public abstract class ParameterlessCommandSpecificationBase<TResult>
        : CanCreateResults, ICommandSpecification<NoParameters, ExecutionResult<TResult>>
    {
        public abstract Task<Response> CanExecuteAsync(NoParameters param, CancellationToken token = default);

        public abstract Task<ExecutionResult<TResult>> ExecuteAsync(NoParameters param, CancellationToken token = default);
    }

    public abstract class CommandSpecificationBase<TParam, TResult>
        : CanCreateResults, ICommandSpecification<TParam, ExecutionResult<TResult>>
    {
        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        public abstract Task<ExecutionResult<TResult>> ExecuteAsync(TParam param, CancellationToken token = default);
    }

    public abstract class CommandSpecificationBase<TParam>
        : CanCreateResults, ICommandSpecification<TParam, ExecutionResult>
    {
        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        public abstract Task<ExecutionResult> ExecuteAsync(TParam param, CancellationToken token = default);
    }
}
