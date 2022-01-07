namespace Plastic.Sample.TodoList.Client.Desktop.ViewModels
{
    using GalaSoft.MvvmLight;
    using Plastic.Sample.TodoList.AppCommands;

    public class TodoViewModel : ViewModelBase
    {
        private readonly int _id;
        private readonly DoneCommand _doneCommand;
        private bool _done;

        public string Title { get; }

        public bool Done
        {
            get => this._done;
            set => SetDone(value);
        }

        public TodoViewModel(
            int id, string title, bool done,
            DoneCommand doneCommand)
        {
            this._id = id;
            this.Title = title;
            this._done = done;
            this._doneCommand = doneCommand;
        }

        protected bool SetDone(bool newValue)
        {
            if (this._done == false)
            {
                var result = this._doneCommand.ExecuteAsync(this._id).Result;
                if (result.HasSucceeded())
                    return Set(ref this._done, true);
            }

            return false;
        }
    }
}
