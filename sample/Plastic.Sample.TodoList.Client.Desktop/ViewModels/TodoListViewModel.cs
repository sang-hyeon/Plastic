namespace Plastic.Sample.TodoList.Client.Desktop.ViewModels
{
    using Plastic.Sample.TodoList.AppCommands.GetAllTodoItems;
    using GalaSoft.MvvmLight;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Threading;
    using GalaSoft.MvvmLight.Views;

    public class TodoListViewModels : ViewModelBase
    {
        private readonly GetAllTodoItemsCommand _getAllTodosCommand;
        private readonly IDialogService _dialogService;
        private readonly ObservableCollection<TodoViewModel> _items;


        public ICommand RefreshCommand { get; }

        public ObservableCollection<TodoViewModel> Items
            => this._items;

        public TodoListViewModels(
            GetAllTodoItemsCommand getAllTodosCommand, IDialogService dialogService)
        {
            this._getAllTodosCommand = getAllTodosCommand;
            this._dialogService = dialogService;
            this._items = new ObservableCollection<TodoViewModel>();

            this.RefreshCommand = new RelayCommand(async () => await ReloadAsync());
        }

        private async Task ReloadAsync(CancellationToken token = default)
        {
            this.Items.Clear();

            ExecutionResult<TodoItemDto[]> result =
                await this._getAllTodosCommand.ExecuteAsync(default, token);

            if (result.HasSucceeded())
            {
                foreach (TodoItemDto dto in result.RequiredValue)
                {
                    this.Items.Add(new TodoViewModel(dto.Title, dto.Done));
                }
            }
            else
            {
                await this._dialogService.ShowError(result.Message, "Alert", "Ok", () => { });
            }
        }
    }
}
