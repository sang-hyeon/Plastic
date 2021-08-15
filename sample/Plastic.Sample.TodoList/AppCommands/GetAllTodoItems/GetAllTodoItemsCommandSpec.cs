namespace Plastic.Sample.TodoList.AppCommands.GetAllTodoItems
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic.Sample.TodoList.Domain;
    using Plastic.Sample.TodoList.ServiceAgents;

    internal class GetAllTodoItemsCommandSpec : ParameterlessCommandSpecificationBase<TodoItemDto[]>
    {
        private readonly ITodoItemRepository _todoRepo;

        public GetAllTodoItemsCommandSpec(ITodoItemRepository repository)
        {
            this._todoRepo = repository;
        }

        public override Task<Response> CanExecuteAsync(NoParameters? _ = default, CancellationToken token = default)
        {
            return CanBeExecutedTask();
        }

        protected override Task<ExecutionResult<TodoItemDto[]>> OnExecuteAsync(
            NoParameters? _ = default, CancellationToken token = default)
        {
            IEnumerable<TodoItem> items = this._todoRepo.GetAll();
            IEnumerable<TodoItemDto> itemsDto = items.Select(item => ToDto(item));

            return SuccessTask(itemsDto.ToArray());
        }

        private static TodoItemDto ToDto(TodoItem item)
        {
            return new TodoItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Done = item.Done
            };
        }
    }
}
