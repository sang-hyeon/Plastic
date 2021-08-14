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
