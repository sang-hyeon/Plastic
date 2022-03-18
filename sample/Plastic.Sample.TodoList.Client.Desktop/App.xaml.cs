namespace Plastic.Sample.TodoList.Client.Desktop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using GalaSoft.MvvmLight.Views;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Plastic.Sample.TodoList.Client.Desktop.Services;
    using Plastic.Sample.TodoList.Client.Desktop.ViewModels;

    [SuppressMessage("Performance", "CA1810:참조 형식 정적 필드 인라인을 초기화하세요.", Justification = "sample")]
    public partial class App : Application
    {
        private readonly static IHost _apphost = default!;

        public static IServiceProvider Provider => _apphost.Services;

        static App()
        {
            // HACK: It's sample, Don't use like this...
            IHostBuilder host = new HostBuilder()
                                                .ConfigureServices(services =>
                                                {
                                                    services.AddTransient<TodoListViewModels>();
                                                    services.AddTransient<IDialogService, DialogService>();

                                                    Plastic.Sample.TodoList.Initializer.Init(services);
                                                    Plastic.Sample.TodoList.Data.Initializer.Init(services);
                                                })
                                                .ConfigureLogging(logging =>
                                                {
                                                    logging.AddConsole();
                                                    logging.AddDebug();
                                                });

            _apphost = host.Build();
        }

        public App()
        {
        }
    }
}
