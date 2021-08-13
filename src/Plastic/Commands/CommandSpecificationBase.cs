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

        protected static Task<ExecutionResult> SuccessTask()
            => Task.FromResult(new ExecutionResult());

        protected static Task<ExecutionResult> FailureTask(string? message)
            => Task.FromResult(new ExecutionResult(false, message));

        protected static Task<Response> CanBeExecutedTask()
            => Task.FromResult(new Response());

        protected static Task<Response> CannotBeExecutedTask(string? message)
            => Task.FromResult(new Response(false, message));

        protected static ExecutionResult Success()
            => new ExecutionResult();

        protected static ExecutionResult Failure(string? message)
            => new ExecutionResult(false, message);

        protected static Response CanBeExecuted()
            => new Response();

        protected static Response CannotBeExecuted(string? message)
            => new Response(false, message);
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
