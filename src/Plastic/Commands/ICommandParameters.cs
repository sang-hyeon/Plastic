namespace Plastic
{
    public interface ICommandParameters<out TResponse>
        where TResponse : Response
    {
    }
}
