namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    internal record TTFFParameter : CommandParameters
    {
    }

    internal record TTFFResponse : Response
    {
        protected internal TTFFResponse(ResponseState state)
            : base(state)
        {
        }
    }

    internal class TTFFCommandSpec : CommandSpecificationBase<TTFFParameter, TTFFResponse>
    {
        public override Task<Response> CanExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return RespondWithSuccess();
        }

        public override Task<TTFFResponse> ExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return Task.FromResult(new TTFFResponse(SuccessState()));
        }
    }
}
