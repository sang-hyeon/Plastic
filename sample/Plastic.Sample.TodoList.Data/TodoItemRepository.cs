namespace Plastic.Sample.TodoList.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Plastic.Sample.TodoList.Domain;
    using Plastic.Sample.TodoList.ServiceAgents;

    [SuppressMessage("", "CA1812", Justification = "타입으로써 사용합니다.")]
    internal class TodoItemRepository : ITodoItemRepository
    {
        protected static readonly HashSet<TodoItem> Items
            = new HashSet<TodoItem>
            {
                new TodoItem(1, "A", "B", false),
                new TodoItem(2, "A", "B", false),
                new TodoItem(3, "A", "B", false),
                new TodoItem(4, "A", "B", false),
                new TodoItem(5, "A", "B", false),
                new TodoItem(6, "A", "B", false),
                new TodoItem(7, "A", "B", false),
            };


        public void Add(string title, string? note)
        {
            int nextId = Items.Max(q => q.Id);
            var newTodo = new TodoItem(nextId, title, note ?? string.Empty, false);
            Items.Add(newTodo);
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return Items;
        }

        public Task SaveChangesAsync(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }

        public void Update(TodoItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}
