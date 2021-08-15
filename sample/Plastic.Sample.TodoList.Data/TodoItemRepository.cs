namespace Plastic.Sample.TodoList.Data
{
    using System.Collections.Generic;
    using Plastic.Sample.TodoList.Domain;
    using Plastic.Sample.TodoList.ServiceAgents;

    public class TodoItemRepository : ITodoItemRepository
    {
        protected static readonly HashSet<TodoItem> Items
            = new HashSet<TodoItem>
            {
                new TodoItem(1, "A", "B", true),
                new TodoItem(2, "A", "B", true),
                new TodoItem(3, "A", "B", true),
                new TodoItem(4, "A", "B", true),
                new TodoItem(5, "A", "B", true),
                new TodoItem(6, "A", "B", true),
                new TodoItem(7, "A", "B", true),
            };


        public void Add(TodoItem item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return Items;
        }

        public void Update(TodoItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}
