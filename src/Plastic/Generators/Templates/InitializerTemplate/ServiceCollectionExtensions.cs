#pragma warning disable
namespace Plastic.Generators
{
    using Microsoft.Extensions.DependencyInjection;
    using Plastic.Commands;

    internal static class ServiceCollectionExtensions
    {
        public static void UsePlastic(this IServiceCollection services, BuildPipeline? pipelineBuilder = default)
        {
            if (pipelineBuilder is not null)
                services.AddTransient<BuildPipeline>(_ => pipelineBuilder);

            AddGeneratedCommands(services);
        }

        private static void AddGeneratedCommands(IServiceCollection services)
        {
            services.AddTransient(typeof(TTFFCommand));
        }
    }
}
#pragma warning restore
