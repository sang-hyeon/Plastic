namespace Plastic.Sample.TodoList.Client.Desktop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Plastic.Sample.TodoList.Client.Desktop.Commands;
    using Plastic.Sample.TodoList.Client.Desktop.Services;
    using Plastic.Sample.TodoList.Client.Desktop.ViewModels;
    using Plastic.Sample.TodoList.Data;
    using Plastic.Sample.TodoList.Pipeline;
    using Plastic.Sample.TodoList.ServiceAgents;

    [SuppressMessage("Performance", "CA1810:참조 형식 정적 필드 인라인을 초기화하세요.", Justification = "sample")]
    public partial class App : Application
    {
        public readonly static IServiceProvider Provider = default!;

        static App()
        {
            // HACK: just sample, don't use like this...

            var services = new ServiceCollection();

            BuildPipeline pipeline = p =>
            {
                return new Pipe[]
                {
                    new LoggingPipe(p.GetRequiredService<ILogger<LoggingPipe>>())
                };
            };
            services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
            services.AddTransient<TodoListViewModels>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<RefreshAllTodosVCommand>();
            services.AddPlastic();

            Provider = services.BuildServiceProvider();
        }

        public App()
        {
        }
    }
}
