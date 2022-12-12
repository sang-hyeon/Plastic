using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlasticCommand;

namespace Plastic.Sample.TodoList.Pipeline;

public class LoggingPipe : IPipe
{
    private readonly ILogger<LoggingPipe> _logger;

    public LoggingPipe(ILogger<LoggingPipe> logger)
    {
        this._logger = logger;
    }

    public async Task<object?> Handle(
        PipelineContext context, Behavior nextBehavior, CancellationToken token)
    {
        this._logger.LogInformation($"Execute Command - {context.CommandSpec.Name}");
        this._logger.LogInformation($"Parameter - {context.Parameter?.ToString()}");

        object? result = await nextBehavior.Invoke().ConfigureAwait(false);

        this._logger.LogInformation($"Result - {result}");

        return result;
    }
}
