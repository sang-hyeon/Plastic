namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    internal record TTFFParameter : CommandParameters
    {
    }

    internal record TTFFResponse : ExecutionResult
    {
        protected internal TTFFResponse(Response response)
            : base(response)
        {
        }
    }

    internal class TTFFCommandSpec : CommandSpecificationBase<TTFFParameter, TTFFResponse>
    {
        public override Task<Response> CanExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return Task.FromResult(new Response());
        }

        public override Task<TTFFResponse> ExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return Task.FromResult(new TTFFResponse(new Response()));
        }
    }
}
