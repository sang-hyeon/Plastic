namespace Plastic.Sample.TodoList.Pipeline
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic;
    using Plastic.Sample.TodoList.ServiceAgents;

    public class TransactionPipe : Pipe
    {
        public override async Task<ExecutionResult> Handle(
            PipelineContext context, Behavior<ExecutionResult> nextBehavior, CancellationToken token)
        {
            ExecutionResult result = await nextBehavior.Invoke()
                                                                      .ConfigureAwait(false);

            await SaveChangesAsync(context, token);

            return result;
        }

        private static async Task SaveChangesAsync(PipelineContext context, CancellationToken token)
        {
            var repository = context.Services.SingleOrDefault(q => q is ITodoItemRepository)
                                                         as ITodoItemRepository;

            if (repository is not null)
                await repository.SaveChangesAsync(token);
        }
    }
}
