namespace Plastic.Sample.TodoList.Domain
{
    public class TodoItem : IEntity<int>
    {
        public int Id { get; }

        public string Title { get; }

        public string Note { get; }

        public bool Done { get; }

        public TodoItem(int id, string title, string note, bool done)
        {
            this.Id = id;
            this.Title = title;
            this.Note = note;
            this.Done = done;
        }
    }
}
