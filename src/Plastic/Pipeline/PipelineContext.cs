namespace Plastic
{
    using System;

    public sealed record PipelineContext
    {
        public readonly CommandParameters Parameters;
        public readonly Type CommandSpec;

        public PipelineContext(CommandParameters parameter, Type commandSpec)
        {
            this.Parameters = parameter;
            this.CommandSpec = commandSpec;
        }
    }
}
