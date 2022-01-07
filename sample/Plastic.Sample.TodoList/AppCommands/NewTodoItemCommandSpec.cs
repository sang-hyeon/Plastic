namespace Plastic.Sample.TodoList.AppCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic;
    using Plastic.Sample.TodoList.ServiceAgents;

    internal class NewTodoItemCommandSpec : CommandSpecificationBase<(string Title, string? Note)>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public NewTodoItemCommandSpec(ITodoItemRepository todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        public override Task<Response> CanExecuteAsync(
            (string Title, string? Note) param, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(param.Title))
                return CannotBeExecutedTask("Blank titles are not allowed.");
            else
                return CanBeExecutedTask();
        }

        protected override Task<ExecutionResult> OnExecuteAsync(
            (string Title, string? Note) param, CancellationToken token = default)
        {
            this._todoItemRepository.Add(param.Title, param.Note);

            return SuccessTask();
        }
    }
}
