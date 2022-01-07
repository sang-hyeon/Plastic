namespace Plastic.Sample.TodoList.ServiceAgents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic.Sample.TodoList.Domain;

    internal interface ITodoItemRepository
    {
        public void Add(string title, string? note);

        public IEnumerable<TodoItem> GetAll();

        public Task SaveChangesAsync(CancellationToken token = default);
    }
}
