using Plastic.Sample.TodoList.ServiceAgents;
using PlasticCommand;
using System.Threading;
using System.Threading.Tasks;

namespace Plastic.Sample.TodoList.AppCommands;

internal class NewTodoItemCommandSpec
    : ICommandSpecificationWithValidation<(string Title, string? Note), bool, bool>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public NewTodoItemCommandSpec(ITodoItemRepository todoItemRepository)
    {
        this._todoItemRepository = todoItemRepository;
    }

    public Task<bool> ExecuteAsync(
        (string Title, string? Note) param, CancellationToken token = default)
    {
        this._todoItemRepository.Add(param.Title, param.Note);

        token.ThrowIfCancellationRequested();
        return Task.FromResult(true);
    }

    public Task<bool> CanExecuteAsync((string Title, string? Note) param, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return Task.FromResult(string.IsNullOrEmpty(param.Title));
    }
}
