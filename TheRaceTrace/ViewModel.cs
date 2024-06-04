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

        private Dictionary<int, LapTime[]>? _lapTimesByLap;

        public ViewModel(ErgastService ergastService)
        {
            _getLapTimesCommand = new DelegateCommand(OnGetLapTimes);
            _ergastService = ergastService;

            Plot = new PlotModel
            {
                Series =
                {
                    CreateSeries("Series 1"),
                    CreateSeries("Series 2"),
                    CreateSeries("Series 3")
                }
            };
        }

        public ICommand GetLapTimesCommand => _getLapTimesCommand;

        public Dictionary<int, LapTime[]>? LapTimesByLap
        {
            get => _lapTimesByLap;
            set => SetProperty(ref _lapTimesByLap, value);
        }

        public PlotModel Plot { get; }

        private static LineSeries CreateSeries(string name)
        {
            return new LineSeries
            {
                Title = name,
                Points =
                {
                    new(0, 0),
                    new(1, 1),
                    new(2, 2)
                }
            };
        }

        private void OnGetLapTimes(object? commandParameter)
        {
            LapTimesByLap = _ergastService.GetLapTimes();
        }

    }
}
