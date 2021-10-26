#pragma warning disable
#nullable enable
namespace Plastic.Commands
{
    using Plastic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TargetResult = Plastic.ExecutionResult<Plastic.TTFFResult>;
    using TargetParameter = TTFFParameter;
    using TargetCommandSpec = Plastic.TTFFCommandSpec;
    using Microsoft.Extensions.DependencyInjection;


    internal sealed class TTFFCommand : ICommandSpecification<TargetParameter, TargetResult>
    {
        private readonly IServiceProvider _provider;

        public TTFFCommand(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public async Task<Response> CanExecuteAsync(
            TargetParameter param, CancellationToken token = default)
        {
            Response response;
            using (IServiceScope scope = this._provider.CreateScope())
            {
                TargetCommandSpec spec = scope.ServiceProvider.GetRequiredService<TargetCommandSpec>();

                response = await spec.CanExecuteAsync(param, token).ConfigureAwait(false);
            }

            return response;
        }

        public async Task<TargetResult> ExecuteAsync(TargetParameter param, CancellationToken token = default)
        {
            TargetResult response;
            using (IServiceScope scope = this._provider.CreateScope())
            {
                BuildPipeline? pipelineBuilder = scope.ServiceProvider.GetService<BuildPipeline>();
                IEnumerable<Pipe> pipeline = pipelineBuilder?.Invoke(scope.ServiceProvider) ?? Array.Empty<Pipe>();
                pipeline = pipeline.Reverse();
                PipelineContext context = CreatePipelineContext(param, scope.ServiceProvider);

                Behavior<ExecutionResult> command = CreateCommandAsBehavior(scope.ServiceProvider, param, token);
                Behavior<ExecutionResult> process =
                    pipeline.Aggregate(command, (next, pipe) => () => pipe.Handle(context, next, token));

                response = (TargetResult)await process.Invoke().ConfigureAwait(false);
            }

            return response;
        }

        private static PipelineContext CreatePipelineContext(TargetParameter param, IServiceProvider provider)
        {
            object?[] services = new object?[]
            {
                provider.GetService<IServiceProvider>() // will be changed on template
            };

            object[] servicesFound = services.OfType<object>().ToArray();

            return new PipelineContext<Plastic.TTFFResult>(param, typeof(TargetCommandSpec), servicesFound);
        }

        private static Behavior<ExecutionResult> CreateCommandAsBehavior(
            IServiceProvider provider, TargetParameter param, CancellationToken token = default)
        {
            return new Behavior<ExecutionResult>(() =>
            {
                TargetCommandSpec spec = provider.GetRequiredService<TargetCommandSpec>();
                return spec.ExecuteAsync(param, token)
                            .ContinueWith(task => (ExecutionResult)task.Result, default, TaskContinuationOptions.None, TaskScheduler.Current);
            });
        }
    }
}
#nullable disable
#pragma warning restore
