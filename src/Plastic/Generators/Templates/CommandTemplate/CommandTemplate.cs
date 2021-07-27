﻿namespace Plastic.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TargetParameter = Plastic.TTFFParameter;
    using TargetResponse = Plastic.TTFFResponse;
    using TargetCommandSpec = Plastic.TTFFCommandSpec;

#nullable enable

    internal sealed class TTFFCommand : ICommandSpecification<TargetParameter, TargetResponse>
    {
        private readonly GetService _provider;

        public TTFFCommand(GetService provider)
        {
            this._provider = provider;
        }

        public Task<Response> CanExecuteAsync(
            TargetParameter param, CancellationToken token = default)
        {
            TargetCommandSpec spec = GetRequiredService<TargetCommandSpec>();

            return spec.CanExecuteAsync(param, token);
        }

        public async Task<TargetResponse> ExecuteAsync(TargetParameter param, CancellationToken token = default)
        {
            BuildPipeline? pipelineBuilder = GetService<BuildPipeline>();
            IEnumerable<IPipe> pipeline = pipelineBuilder?.Invoke() ?? Array.Empty<IPipe>();
            var context = new PipelineContext(param, typeof(TargetCommandSpec));

            Behavior<Response> command = CreateCommandAsBehavior(param, token);
            Behavior<Response> process =
                pipeline.Aggregate(command, (next, pipe) => () => pipe.Handle(context, next, token));

            return (TargetResponse)await process.Invoke().ConfigureAwait(false);
        }

        private Behavior<Response> CreateCommandAsBehavior(
            TargetParameter param, CancellationToken token = default)
        {
            return new Behavior<Response>(() =>
            {
                TargetCommandSpec spec = GetRequiredService<TargetCommandSpec>();
                return spec.ExecuteAsync(param, token)
                            .ContinueWith(task => (Response)task.Result, default, TaskContinuationOptions.None, TaskScheduler.Current);
            });
        }

        private T GetRequiredService<T>()
        {
            return GetService<T>() ?? throw new InvalidOperationException();
        }

        private T? GetService<T>()
        {
            object? nullableService = this._provider.Invoke(typeof(T));

            if (nullableService is T service)
                return service;
            else
                return default;
        }
    }
#nullable disable
}
