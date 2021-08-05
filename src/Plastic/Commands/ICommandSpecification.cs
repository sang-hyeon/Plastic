namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICommandSpecification<in TParam, TResponse>
        where TParam : CommandParameters
        where TResponse : Response
    {
        Task<TResponse> ExecuteAsync(TParam param, CancellationToken token = default);

        Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);
    }

    public interface ICommandSpecification<TResponse>
        : ICommandSpecification<NoParameters, TResponse>
        where TResponse : Response
    {
    }

    public interface ICommandSpecification
        : ICommandSpecification<NoParameters, Response>
    {
    }
}
