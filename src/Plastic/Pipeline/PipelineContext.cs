namespace Plastic
{
    using System;
    using System.Collections.Generic;

    public sealed record PipelineContext
    {
        public readonly CommandParameters Parameters;
        public readonly Type CommandSpec;
        public readonly IReadOnlyList<object> Services;

        public PipelineContext(
            CommandParameters parameter, Type commandSpec, IReadOnlyList<object> services)
        {
            this.Parameters = parameter;
            this.CommandSpec = commandSpec;
            this.Services = services;
        }
    }
}
