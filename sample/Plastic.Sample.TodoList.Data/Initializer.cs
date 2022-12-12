using Microsoft.Extensions.DependencyInjection;
using Plastic.Sample.TodoList.ServiceAgents;

namespace Plastic.Sample.TodoList.Data;

public static class Initializer
{
    public static void Init(IServiceCollection collection)
    {
        collection.AddScoped<ITodoItemRepository, TodoItemRepository>();
    }
}
