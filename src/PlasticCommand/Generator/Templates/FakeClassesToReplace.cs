#pragma warning disable
namespace PlasticCommand.Generator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed record TTFFParameter
    {
    }

    internal sealed record TTFFResult
    {
    }

    internal sealed record TTFFValidationResult
    {
    }

    internal sealed class TTFFCommandSpec : ICommandSpecification<TTFFParameter, TTFFResult>
    {
        public Task<TTFFResult> ExecuteAsync(TTFFParameter param, CancellationToken token = default)
        {
            return default!; // fake
        }
    }

    internal sealed class TTFFValidatableCommandSpec
        : ICommandSpecificationWithValidation<TTFFParameter, TTFFResult, TTFFValidationResult>
    {
        public Task<TTFFValidationResult> CanExecuteAsync(
            TTFFParameter param, CancellationToken token = default) => throw new NotImplementedException();

        public Task<TTFFResult> ExecuteAsync(
            TTFFParameter param, CancellationToken token = default)
            => throw new System.NotImplementedException();
    }
}
#pragma warning restore
