namespace Plastic.Sample.TodoList.Domain
{
    internal class TodoItem : IEntity<int>
    {
        public int Id { get; }

        public string Title { get; }

        public string? Note { get; }

        public bool IsDone { get; private set; }

        public TodoItem(int id, string title, string? note, bool done)
        {
            this.Id = id;
            this.Title = title;
            this.Note = note;
            this.IsDone = done;
        }

        public void Done()
        {
            this.IsDone = true;
        }

        public void TodoAgain()
        {
            this.IsDone= false;
        }

        public static TodoItem NewTodo(int id, string title, string? note = default)
        {
            return new TodoItem(id, title, note, false);
        }
    }
}
