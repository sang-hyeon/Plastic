using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Plastic.Sample.TodoList.Domain;

namespace Plastic.Sample.TodoList.ServiceAgents;

internal interface ITodoItemRepository
{
    public void Add(string title, string? note);

    public IEnumerable<TodoItem> GetAll();

    public bool Exists(int id);

    public Task SaveChangesAsync(CancellationToken token = default);
}
