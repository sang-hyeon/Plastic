namespace Plastic.Sample.TodoList.ServiceAgents
{
    using System.Collections.Generic;
    using Plastic.Sample.TodoList.Domain;

    public interface ITodoItemRepository
    {
        public void Add(TodoItem item);

        public void Update(TodoItem item);

        public IEnumerable<TodoItem> GetAll();
    }
}
