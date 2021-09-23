namespace Plastic.Sample.TodoList.Client.Desktop.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using Plastic.Sample.TodoList.Client.Desktop.Commands;

    public class TodoListViewModels : ViewModelBase
    {
        public ICommand RefreshCommand { get; }

        public ObservableCollection<TodoViewModel> Items { get; }

        public TodoListViewModels(RefreshAllTodosVCommand refreshAllTodosVCommand)
        {
            this.Items = new ObservableCollection<TodoViewModel>();
            this.RefreshCommand = refreshAllTodosVCommand;
            refreshAllTodosVCommand.SetParameter(this.Items);
        }
    }
}
