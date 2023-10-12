#pragma warning disable
namespace PlasticCommand
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    internal static class ServiceCollectionExtensions
    {
        public static void AddPlastic(this IServiceCollection services, BuildPipeline? pipelineBuilder = default)
        {
            if (pipelineBuilder is not null)
                services.AddTransient<BuildPipeline>(_ => pipelineBuilder);

            AddGeneratedCommands(services);
            AddGeneratedCommandGroups(services);
        }

        private static void AddGeneratedCommands(IServiceCollection services)
        {
            services.AddTransient(typeof(Templates.TTFFCommand)); // replace: on template
        }

        private static void AddGeneratedCommandGroups(IServiceCollection services)
        {
            // {{CommandGroups}}
        }
    }
}
#pragma warning restore
