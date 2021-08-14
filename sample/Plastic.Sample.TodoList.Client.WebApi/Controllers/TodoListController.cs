namespace Plastic.Sample.TodoList.Client.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Plastic.Sample.TodoList.AppCommands.GetAllTodoItems;

    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        [HttpGet]
        public async Task<TodoItemDto[]?> GetAll(
            [FromServices] GetAllTodoItemsCommand command,
            [FromServices] ILogger<TodoListController> logger)
        {
            ExecutionResult<TodoItemDto[]> result = await command.ExecuteAsync(default);
            if (result.HasSucceeded())
            {
                return result.RequiredValue;
            }
            else
            {
                logger.LogWarning(result.Message);
                return null;
            }
        }
    }
}
