using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TheRaceTrace
{
    public class DelegateCommand(Action<object?> executeAction) : ICommand
    {
        private readonly Action<object?> _executeAction = executeAction;

        public void Execute(object? parameter) => _executeAction(parameter);

        public bool CanExecute(object? parameter) => true;

        public event EventHandler? CanExecuteChanged;
    }
}
