using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Plastic.Sample.TodoList.AppCommands.Dto;
using Plastic.Sample.TodoList.Domain;
using Plastic.Sample.TodoList.ServiceAgents;
using PlasticCommand;

namespace Plastic.Sample.TodoList.AppCommands;

internal class GetAllTodoItemsCommandSpec
    : ICommandSpecificationWithValidation<Voidy?, TodoItemDto[], bool>
{
    private readonly ITodoItemRepository _todoRepo;

    public GetAllTodoItemsCommandSpec(ITodoItemRepository repository)
    {
        this._todoRepo = repository;
    }

    public Task<bool> CanExecuteAsync(Voidy? param, CancellationToken token = default)
    {
        return Task.FromResult(true);
    }

    public Task<TodoItemDto[]> ExecuteAsync(Voidy? param, CancellationToken token = default)
    {
        IEnumerable<TodoItem> items = this._todoRepo.GetAll();
        IEnumerable<TodoItemDto> itemsDto = items.Select(ToDto);

        return Task.FromResult(itemsDto.ToArray());
    }

    private static TodoItemDto ToDto(TodoItem item)
    {
        return new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Done = item.IsDone
        };
    }
}
