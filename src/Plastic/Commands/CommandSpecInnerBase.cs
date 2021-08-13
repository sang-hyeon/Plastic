namespace Plastic
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class CommandSpecInnerBase
    {
        protected static ExecutionResult Success()
               => new ExecutionResult(true);

        protected static ExecutionResult<T> Success<T>(T value)
               => new ExecutionResult<T>(true, default, value);

        protected static Task<ExecutionResult> SuccessTask()
            => Task.FromResult(new ExecutionResult(true));

        protected static Task<ExecutionResult<T>> SuccessTask<T>(T value)
            => Task.FromResult(new ExecutionResult<T>(true, default, value));

        protected static ExecutionResult Failure(string? message)
            => new ExecutionResult(false, message);

        protected static ExecutionResult<T> Failure<T>(string? message)
            => new ExecutionResult<T>(false, message, default);

        protected static Task<ExecutionResult> FailureTask(string? message)
            => Task.FromResult(new ExecutionResult(false, message));

        protected static Task<ExecutionResult<T>> FailureTask<T>(string? message)
            => Task.FromResult(new ExecutionResult<T>(false, message));

        protected static Task<Response> CanBeExecutedTask()
            => Task.FromResult(new Response());

        protected static Task<Response> CannotBeExecutedTask(string? message)
            => Task.FromResult(new Response(false, message));

        protected static Response CanBeExecuted()
            => new Response();

        protected static Response CannotBeExecuted(string? message)
            => new Response(false, message);
    }
}
