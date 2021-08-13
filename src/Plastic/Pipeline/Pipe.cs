namespace Plastic
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class Pipe : CanCreateResults
    {
        public Pipe()
        {
        }

        public abstract Task<ExecutionResult> Handle(
            PipelineContext context, Behavior<ExecutionResult> nextBehavior, CancellationToken token);
    }

    public delegate Task<TResult> Behavior<TResult>()
        where TResult : ExecutionResult;
}
