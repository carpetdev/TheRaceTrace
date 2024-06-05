using OxyPlot;
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
        private readonly DelegateCommand _getRaceTraceCommand;
        private readonly ErgastService _ergastService;
        private readonly ChartService _chartService;

        private PlotModel? _plot;

        public ViewModel(ErgastService ergastService, ChartService chartService)
        {
            _getRaceTraceCommand = new DelegateCommand(OnGetRaceTrace, CanGetRaceTrace);
            _ergastService = ergastService;
            _chartService = chartService;
        }

        public ICommand GetRaceTraceCommand => _getRaceTraceCommand;

        public PlotModel? Plot
        {
            get => _plot;
            set => SetProperty(ref _plot, value);
        }

        private bool ErgastTimeout { get; set; }

        private async void OnGetRaceTrace(object? commandParameter)
        {
            SortedDictionary<int, LapTime[]> lapTimesByLap = _ergastService.GetLapTimes();
            Plot = _chartService.CreateTrace(lapTimesByLap);
            ErgastTimeout = true;
            _getRaceTraceCommand.RaiseCanExecuteChanged();
            await Task.Delay(20_000);
            ErgastTimeout = false;
            _getRaceTraceCommand.RaiseCanExecuteChanged();
        }

        private bool CanGetRaceTrace(object? commandParameter) => !ErgastTimeout;
    }
}
