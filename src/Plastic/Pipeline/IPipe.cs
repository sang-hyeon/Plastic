namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPipe
    {
        Task<Response> Handle(
            PipelineContext context, Behavior<Response> nextBehavior, CancellationToken token);
    }

    public delegate Task<TResponse> Behavior<TResponse>()
        where TResponse : Response;
}
