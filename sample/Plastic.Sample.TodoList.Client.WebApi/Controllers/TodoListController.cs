using Microsoft.AspNetCore.Mvc;
using Plastic.Sample.TodoList.AppCommands;
using Plastic.Sample.TodoList.AppCommands.Dto;
using System.Threading.Tasks;

namespace Plastic.Sample.TodoList.Client.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoListController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllTodoItemsCommand command)
    {
        TodoItemDto[] result = await command.ExecuteAsync(default);
        return Ok(result);
    }

    [HttpPut("Done/{todoItemId}")]
    public async Task<IActionResult> Done(
        [FromServices] DoneCommand command,
        int todoItemId)
    {
        _ = await command.ExecuteAsync(todoItemId);
        return Ok();
    }
}
