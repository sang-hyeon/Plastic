namespace Plastic
{
    public abstract class ParameterlessCommandSpecificationBase
        : InternalCommandSpecificationBase<NoParameters?, ExecutionResult>
    {

        protected override ExecutionResult CreateFailure(string? message)
            => Failure(message);
    }

    public abstract class ParameterlessCommandSpecificationBase<TResult>
        : InternalCommandSpecificationBase<NoParameters?, ExecutionResult<TResult>>
    {
        protected override ExecutionResult<TResult> CreateFailure(string? message)
               => Failure<TResult>(message);
    }

    public abstract class CommandSpecificationBase<TParam, TResult>
        : InternalCommandSpecificationBase<TParam, ExecutionResult<TResult>>
    {
        protected override ExecutionResult<TResult> CreateFailure(string? message)
               => Failure<TResult>(message);
    }

    public abstract class CommandSpecificationBase<TParam>
        : InternalCommandSpecificationBase<TParam, ExecutionResult>
    {
        protected override ExecutionResult CreateFailure(string? message)
               => Failure(message);
    }
}
