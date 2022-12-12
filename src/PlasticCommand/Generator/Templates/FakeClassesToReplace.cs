#pragma warning disable
namespace PlasticCommand.Generator
{
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed record TTFFParameter
    {
    }

    internal sealed record TTFFResult
    {
    }

    internal sealed class TTFFCommandSpec : ICommandSpecification<TTFFParameter, TTFFResult>
    {
        public Task<TTFFResult> ExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return default!; // fake
        }
    }
}
#pragma warning restore
