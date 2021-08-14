namespace Plastic.Sample.TodoList.Client.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Plastic.Sample.TodoList.Data;
    using Plastic.Sample.TodoList.Pipeline;
    using Plastic.Sample.TodoList.ServiceAgents;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITodoItemRepository, TodoItemRepository>();
            services.AddControllers();

            BuildPipeline pipeline = p =>
            {
                return new Pipe[]
                {
                    new LoggingPipe(p.GetRequiredService<ILogger<LoggingPipe>>())
                };
            };
            services.AddPlastic(pipeline);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
