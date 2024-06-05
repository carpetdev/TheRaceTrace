using System.Windows.Input;

namespace TheRaceTrace
{
    public class DelegateCommand(Action<object?> executeAction, Func<object?, bool> canExecuteAction) : ICommand
    {
        private readonly Action<object?> _executeAction = executeAction;
        private readonly Func<object?, bool> _canExecuteAction = canExecuteAction;

        public void Execute(object? parameter) => _executeAction(parameter);

        public bool CanExecute(object? parameter) => _canExecuteAction.Invoke(parameter);

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
