namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class CommandSpecificationBase<TParam, TResponse>
        : ICommandSpecification<TParam, TResponse>
        where TParam : CommandParameters
        where TResponse : Response
    {
        public abstract Task<Response> CanExecuteAsync(TParam request, CancellationToken token = default);

        public abstract Task<TResponse> ExecuteAsync(TParam request, CancellationToken token = default);

        protected static Task<T> RespondWith<T>(T item)
            where T : Response
        {
            return Task.FromResult(item);
        }

        protected static Task<Response> RespondWithSuccess()
        {
            return Task.FromResult(Success());
        }

        protected static Task<Response> RespondWithFailure(string? message = default)
        {
            return Task.FromResult(Failure(message));
        }

        protected static Response Success()
        {
            return new Response(true, null);
        }

        protected static Response Failure(string? message = null)
        {
            return new Response(false, message);
        }

        protected static ResponseState SuccessState()
        {
            return new ResponseState(true, null);
        }

        protected static ResponseState FailureState(string? message = null)
        {
            return new ResponseState(false, message);
        }
    }

    public abstract class CommandSpecificationBase<TResponse>
        : CommandSpecificationBase<NoParameters, TResponse>
        where TResponse : Response
    {
    }

    public abstract class CommandSpecificationBase
        : CommandSpecificationBase<Response>
    {
    }
}
