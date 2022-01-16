namespace Plastic
{
    using System;
    using System.Collections.Generic;

    public class PipelineContext
    {
        public readonly object? Parameter;
        public readonly Type CommandSpec;
        public readonly IReadOnlyList<object> Services;

        public PipelineContext(
            object? parameter, Type commandSpec, IReadOnlyList<object> services)
        {
            this.Parameter = parameter;
            this.CommandSpec = commandSpec;
            this.Services = services;
        }

        public virtual ExecutionResult CreateFailure(string? message = default)
        {
            return new ExecutionResult(false, message);
        }
    }

    public sealed class PipelineContext<TCommandResult> : PipelineContext
    {
        public PipelineContext(
            object? parameter, Type commandSpec, IReadOnlyList<object> services)
            : base(parameter, commandSpec, services)
        {
        }

        public override ExecutionResult CreateFailure(string? message = default)
        {
            return new ExecutionResult<TCommandResult>(false, message);
        }
    }
}
