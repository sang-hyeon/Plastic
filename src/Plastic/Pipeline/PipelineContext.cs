namespace Plastic
{
    using System;
    using System.Collections.Generic;

    public sealed record PipelineContext
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
    }
}
