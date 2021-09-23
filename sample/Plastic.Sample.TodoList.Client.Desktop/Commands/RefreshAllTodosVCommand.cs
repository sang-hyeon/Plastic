namespace Plastic.Sample.TodoList.Client.Desktop.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Plastic.Sample.TodoList.Client.Desktop.ViewModels;
    using Plastic.Sample.TodoList.AppCommands.GetAllTodoItems;
    using GalaSoft.MvvmLight.Views;

    public class RefreshAllTodosVCommand : ICommand
    {
        private readonly GetAllTodoItemsCommand _command;
        private readonly IDialogService _dialogService;
        private ICollection<TodoViewModel>? _currentParameter;

        public RefreshAllTodosVCommand(GetAllTodoItemsCommand command, IDialogService dialogService)
        {
            this._command = command;
            this._dialogService = dialogService;
        }

        public event EventHandler? CanExecuteChanged;

        public void SetParameter(ICollection<TodoViewModel> todos)
        {
            this._currentParameter = todos;
        }

        public bool CanExecute(object? parameter)
        {
            // HACK: do not use like this.
            return this._command.CanExecuteAsync(default).Result;
        }

        public void Execute(object? parameter)
        {
            parameter ??= this._currentParameter;
            if (parameter is ICollection<TodoViewModel> collection)
            {
                // HACK: do not use like this.
                ExecutionResult<TodoItemDto[]> result = this._command.ExecuteAsync(default).Result;

                if (result.HasSucceeded())
                {
                    RefreshAll(collection, result.RequiredValue);
                }
                else
                {
                    this._dialogService.ShowError(result.Message, "Alert", "Ok", null);
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        private static void RefreshAll(ICollection<TodoViewModel> targetCollection, IEnumerable<TodoItemDto> dto)
        {
            targetCollection.Clear();

            foreach (TodoItemDto todoDto in dto)
            {
                var todoItem = new TodoViewModel(todoDto.Title, todoDto.Done);
                targetCollection.Add(todoItem);
            }
        }
    }
}
