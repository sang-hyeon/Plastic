namespace Plastic
{
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class InternalCommandSpecificationBase<TParam, TResult>
        : CanCreateResults, ICommandSpecification<TParam, TResult>
        where TResult : ExecutionResult
    {
        internal InternalCommandSpecificationBase()
        {
        }

        public abstract Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);

        protected abstract Task<TResult> OnExecuteAsync(TParam param, CancellationToken token = default);

        public async virtual Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default)
        {
            Response canExecute = await CanExecuteAsync(param, token).ConfigureAwait(false);

            if (canExecute)
            {
                return await OnExecuteAsync(param, token);
            }
            else
            {
                return CreateFailure(canExecute.Message);
            }
        }

        protected abstract TResult CreateFailure(string? message);
    }
}
