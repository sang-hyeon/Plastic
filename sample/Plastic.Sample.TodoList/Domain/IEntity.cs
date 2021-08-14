namespace Plastic.Sample.TodoList.Domain
{
    public interface IEntity<T>
    {
        public T Id { get; }
    }
}
