
using Plastic.Sample.TodoList.ServiceAgents;
using PlasticCommand;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plastic.Sample.TodoList.Pipeline;

public class TransactionPipe : IPipe
{
    public async Task<object?> Handle(
        PipelineContext context, Behavior nextBehavior, CancellationToken token)
    {
        object? result = await nextBehavior.Invoke().ConfigureAwait(false);

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
