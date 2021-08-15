namespace Plastic.Sample.TodoList.Client.Desktop.ViewModels
{
    using GalaSoft.MvvmLight;

    public class TodoViewModel : ViewModelBase
    {
        public string Title { get; }

        public bool Done { get; }

        public TodoViewModel(string title, bool done)
        {
            this.Title = title;
            this.Done = done;
        }
    }
}
