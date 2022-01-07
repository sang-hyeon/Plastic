namespace Plastic.Sample.TodoList.Client.Desktop.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Plastic.Sample.TodoList.Client.Desktop.ViewModels;
    using GalaSoft.MvvmLight.Views;
    using Plastic.Sample.TodoList.AppCommands.Dto;
    using Plastic.Sample.TodoList.AppCommands;

    public class RefreshAllTodosVCommand : ICommand
    {
        private readonly GetAllTodoItemsCommand _command;
        private readonly DoneCommand _doneCommand;
        private readonly IDialogService _dialogService;
        private ICollection<TodoViewModel>? _currentParameter;

        public RefreshAllTodosVCommand(
            GetAllTodoItemsCommand command,
            DoneCommand doneCommand,
            IDialogService dialogService)
        {
            this._command = command;
            this._doneCommand = doneCommand;
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

        private void RefreshAll(ICollection<TodoViewModel> targetCollection, IEnumerable<TodoItemDto> dto)
        {
            targetCollection.Clear();

            foreach (TodoItemDto todoDto in dto)
            {
                var todoItem = new TodoViewModel(todoDto.Id, todoDto.Title, todoDto.Done, this._doneCommand);
                targetCollection.Add(todoItem);
            }
        }
    }
}
