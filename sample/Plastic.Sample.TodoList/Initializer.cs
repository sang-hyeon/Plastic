namespace Plastic.Sample.TodoList
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Plastic.Sample.TodoList.Pipeline;

    public static class Initializer
    {
        public static void Init(IServiceCollection collection)
        {
            BuildPipeline pipeline = p =>
            {
                return new Pipe[]
                {
                    new LoggingPipe(p.GetRequiredService<ILogger<LoggingPipe>>()),
                    new TransactionPipe()
                };
            };

            collection.AddPlastic(pipeline);
        }
    }
}
