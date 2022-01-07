namespace Plastic.Sample.TodoList.Client.WebApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Plastic.Sample.TodoList.AppCommands;
    using Plastic.Sample.TodoList.AppCommands.Dto;

    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] GetAllTodoItemsCommand command)
        {
            ExecutionResult<TodoItemDto[]> result = await command.ExecuteAsync(default);
            if (result.HasSucceeded())
            {
                return Ok(result.RequiredValue);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
        }

        [HttpPut("Done/{todoItemId}")]
        public async Task<IActionResult> Done(
            [FromServices] DoneCommand command,
            int todoItemId)
        {
            ExecutionResult result = await command.ExecuteAsync(todoItemId);

            if (result.HasSucceeded())
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
    }
}
