using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plastic.Sample.TodoList.AppCommands;
using Plastic.Sample.TodoList.AppCommands.Dto;

namespace Plastic.Sample.TodoList.Client.Desktop.ViewModels;

public class TodoListViewModels : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly GetAllTodoItemsCommand _getAllTodoItemsCommand;
    private readonly DoneCommand _doneCommand;
    private readonly TodoAgainCommand _againCommand;

    public ICommand RefreshCommand { get; }

    public ObservableCollection<TodoViewModel> Items { get; }

    public TodoListViewModels(
        GetAllTodoItemsCommand getAllTodoItemsCommand, DoneCommand doneCommand,
        TodoAgainCommand againCommand, IDialogService dialogService)
    {
        this._getAllTodoItemsCommand = getAllTodoItemsCommand;
        this._dialogService = dialogService;
        this._doneCommand = doneCommand;
        this._againCommand = againCommand;

        this.Items = new ObservableCollection<TodoViewModel>();
        this.RefreshCommand = new RelayCommand(RefreshAllTodoItemsViaInnerCommand);
    }

    protected void RefreshAllTodoItemsViaInnerCommand()
    {
        // HACK: It's sample, Don't use like this...
        TodoItemDto[] result = this._getAllTodoItemsCommand.ExecuteAsync(default).Result;
        foreach (TodoItemDto todo in result)
        {
            var todoItem = new TodoViewModel(
                                                todo.Id, todo.Title, todo.Done,
                                                this._doneCommand, this._againCommand);

            this.Items.Add(todoItem);
        }
    }
}
