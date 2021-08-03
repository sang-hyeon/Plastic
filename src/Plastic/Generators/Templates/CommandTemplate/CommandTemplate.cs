namespace Plastic.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TargetParameter = Plastic.TTFFParameter;
    using TargetResponse = Plastic.TTFFResponse;
    using TargetCommandSpec = Plastic.TTFFCommandSpec;
    using Microsoft.Extensions.DependencyInjection;

#nullable enable

    internal sealed class TTFFCommand : ICommandSpecification<TargetParameter, TargetResponse>
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

                response = await spec.CanExecuteAsync(param, token);
            }

            return response;
        }

        public async Task<TargetResponse> ExecuteAsync(TargetParameter param, CancellationToken token = default)
        {
            TargetResponse response;
            using (IServiceScope scope = this._provider.CreateScope())
            {
                BuildPipeline? pipelineBuilder = scope.ServiceProvider.GetService<BuildPipeline>();
                IEnumerable<IPipe> pipeline = pipelineBuilder?.Invoke(scope.ServiceProvider) ?? Array.Empty<IPipe>();
                pipeline = pipeline.Reverse();
                PipelineContext context = CreatePipelineContext(param, scope.ServiceProvider);

                Behavior<Response> command = CreateCommandAsBehavior(scope.ServiceProvider, param, token);
                Behavior<Response> process =
                    pipeline.Aggregate(command, (next, pipe) => () => pipe.Handle(context, next, token));

                response = (TargetResponse)await process.Invoke().ConfigureAwait(false);
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

            return new PipelineContext(param, typeof(TargetCommandSpec), servicesFound);
        }

        private static Behavior<Response> CreateCommandAsBehavior(
            IServiceProvider provider, TargetParameter param, CancellationToken token = default)
        {
            return new Behavior<Response>(() =>
            {
                TargetCommandSpec spec = provider.GetRequiredService<TargetCommandSpec>();
                return spec.ExecuteAsync(param, token)
                            .ContinueWith(task => (Response)task.Result, default, TaskContinuationOptions.None, TaskScheduler.Current);
            });
        }
    }
#nullable disable
}
