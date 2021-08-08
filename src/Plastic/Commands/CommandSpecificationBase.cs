namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class CommandSpecificationBase<TParam, TResult>
        : ICommandSpecification<TParam, TResult>
        where TParam : CommandParameters
        where TResult : ExecutionResult
    {
        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        public abstract Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default);

        protected static Task<ExecutionResult> Success()
        {
            return Task.FromResult(new ExecutionResult());
        }

        protected static Task<ExecutionResult> Failure(string? message)
        {
            return Task.FromResult(new ExecutionResult(false, message));
        }

        protected static Task<Response> CanBeExecuted()
        {
            return Task.FromResult(new Response());
        }

        protected static Task<Response> CannotBeExecuted(string? message)
        {
            return Task.FromResult(new Response(false, message));
        }
    }

    public abstract class CommandSpecificationBase<TResult>
        : CommandSpecificationBase<NoParameters, TResult>
        where TResult : ExecutionResult
    {
    }

    public abstract class CommandSpecificationBase
        : CommandSpecificationBase<ExecutionResult>
    {
    }
}
