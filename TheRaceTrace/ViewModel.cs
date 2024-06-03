using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TheRaceTrace
{
    public class ViewModel : ViewModelBase
    {
        private readonly DelegateCommand _getLapTimesCommand;
        private readonly ErgastService _ergastService;

        public ViewModel(ErgastService ergastService)
        {
            _getLapTimesCommand = new DelegateCommand(OnGetLapTimes);
            _ergastService = ergastService;
        }

        public ICommand GetLapTimesCommand => _getLapTimesCommand;

        private void OnGetLapTimes(object? commandParameter)
        {
            _ergastService.GetLapTimes();
        }

    }
}
