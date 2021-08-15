namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    internal record TTFFParameter
    {
    }

    internal record TTFFResult
    {
    }

    internal class TTFFCommandSpec : CommandSpecificationBase<TTFFParameter, TTFFResult>
    {
        public override Task<Response> CanExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return CanBeExecutedTask();
        }

        protected override Task<ExecutionResult<TTFFResult>> OnExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return SuccessTask(new TTFFResult());
        }
    }
}
