namespace Plastic
{
    public abstract class ParameterlessCommandSpecificationBase
        : InternalCommandSpecificationBase<NoParameters?, ExecutionResult>
    {
    }

    public abstract class ParameterlessCommandSpecificationBase<TResult>
        : InternalCommandSpecificationBase<NoParameters?, ExecutionResult<TResult>>
    {
    }

    public abstract class CommandSpecificationBase<TParam, TResult>
        : InternalCommandSpecificationBase<TParam, ExecutionResult<TResult>>
    {
    }

    public abstract class CommandSpecificationBase<TParam>
        : InternalCommandSpecificationBase<TParam, ExecutionResult>
    {
    }
}
