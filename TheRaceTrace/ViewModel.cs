using System.Windows.Input;

using OxyPlot;

namespace TheRaceTrace
{
    public class ViewModel : ViewModelBase
    {
        private readonly DelegateCommand _getRaceTraceCommand;
        private readonly IErgastService _ergastService;
        private readonly IChartService _chartService;

        private PlotModel? _plot;

        public ViewModel(IErgastService ergastService, IChartService chartService)
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
