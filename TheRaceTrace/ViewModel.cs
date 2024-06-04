using OxyPlot;
using OxyPlot.Series;
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
        private readonly ChartService _chartService;

        private PlotModel? _plot;

        public ViewModel(ErgastService ergastService, ChartService chartService)
        {
            _getLapTimesCommand = new DelegateCommand(OnGetLapTimes);
            _ergastService = ergastService;
            _chartService = chartService;
        }

        public ICommand GetLapTimesCommand => _getLapTimesCommand;

        public PlotModel? Plot
        {
            get => _plot;
            set => SetProperty(ref _plot, value);
        }

        private void OnGetLapTimes(object? commandParameter)
        {
            SortedDictionary<int, LapTime[]> lapTimesByLap = _ergastService.GetLapTimes();
            Plot = _chartService.CreateTrace(lapTimesByLap);
        }

    }
}
