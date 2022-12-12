using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Plastic.Sample.TodoList.Domain;
using Plastic.Sample.TodoList.ServiceAgents;
using PlasticCommand;

namespace Plastic.Sample.TodoList.AppCommands;

internal class TodoAgainCommandSpec : ICommandSpecificationWithValidation<int, bool, bool>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoAgainCommandSpec(ITodoItemRepository todoItemRepository)
    {
        this._todoItemRepository = todoItemRepository;
    }

    public Task<bool> CanExecuteAsync(int todoItemId, CancellationToken token = default)
    {
        bool exists = this._todoItemRepository.Exists(todoItemId);

        token.ThrowIfCancellationRequested();
        return Task.FromResult(exists);
    }

    public Task<bool> ExecuteAsync(int todoItemId, CancellationToken token = default)
    {
        TodoItem todoItem = this._todoItemRepository.GetAll().Single(q => q.Id == todoItemId);
        todoItem.TodoAgain();

        token.ThrowIfCancellationRequested();
        return Task.FromResult(true);
    }
}
