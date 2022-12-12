namespace Plastic.Sample.TodoList.AppCommands.Dto;

public record TodoItemDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public bool Done { get; init; }
}
