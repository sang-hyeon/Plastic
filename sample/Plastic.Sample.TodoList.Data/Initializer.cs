namespace Plastic.Sample.TodoList.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using Plastic.Sample.TodoList.ServiceAgents;

    public static class Initializer
    {
        public static void Init(IServiceCollection collection)
        {
            collection.AddScoped<ITodoItemRepository, TodoItemRepository>();
        }
    }
}
