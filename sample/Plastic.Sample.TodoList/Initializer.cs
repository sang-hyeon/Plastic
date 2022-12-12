using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plastic.Sample.TodoList.Pipeline;
using PlasticCommand;

namespace Plastic.Sample.TodoList;

public static class Initializer
{
    public static void Init(IServiceCollection collection)
    {
        BuildPipeline pipeline = provider =>
        {
            return new IPipe[]
            {
                new LoggingPipe(provider.GetRequiredService<ILogger<LoggingPipe>>()),
                new TransactionPipe()
            };
        };

        collection.AddPlastic(pipeline);
    }
}
