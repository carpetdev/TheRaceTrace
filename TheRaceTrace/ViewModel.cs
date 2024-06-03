using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TheRaceTrace
{
    internal class ViewModel : ViewModelBase
    {
        private readonly DelegateCommand _getLapTimesCommand;
        public ICommand GetLapTimesCommand => _getLapTimesCommand;

        public ViewModel()
        {
            _getLapTimesCommand = new DelegateCommand(OnGetLapTimes);
        }

        private void OnGetLapTimes(object? commandParameter)
        {
            // Do the boi
        }

    }
}
