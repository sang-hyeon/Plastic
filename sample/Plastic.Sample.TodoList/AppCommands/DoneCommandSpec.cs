namespace Plastic.Sample.TodoList.AppCommands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic.Sample.TodoList.Domain;
    using Plastic.Sample.TodoList.ServiceAgents;

    internal class DoneCommandSpec : CommandSpecificationBase<int>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public DoneCommandSpec(ITodoItemRepository todoItemRepository)
        {
            this._todoItemRepository = todoItemRepository;
        }

        public override Task<Response> CanExecuteAsync(int todoItemId, CancellationToken token = default)
        {
            bool exists = this._todoItemRepository.GetAll().Any(q => q.Id == todoItemId);

            if (exists)
                return CanBeExecutedTask();
            else
                return CannotBeExecutedTask("Doesn't exist");
        }

        protected override Task<ExecutionResult> OnExecuteAsync(int todoItemId, CancellationToken token = default)
        {
            TodoItem todoItem = this._todoItemRepository.GetAll().Single(q => q.Id == todoItemId);
            todoItem.Done();

            return SuccessTask();
        }
    }
}
