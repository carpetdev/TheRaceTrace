using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRaceTrace
{
    public class ChartService
    {
        public PlotModel CreateTrace(SortedDictionary<int, LapTime[]> lapTimesByLap)
        {
            int lapCount = lapTimesByLap.Count;
            string winner = lapTimesByLap[lapCount]
                .OrderBy(lapTime => lapTime.Position)
                .First()
                .DriverId;

            double winnerAverageTime = lapTimesByLap.Values
                .SelectMany(lapTimes => lapTimes)
                .Where(lapTime => lapTime.DriverId == winner)
                .Select(lapTime => (lapTime.Time).TotalSeconds)
                .Average();

            Dictionary<string, LineSeries> series = lapTimesByLap[1]
                .Select(lapTime => new LineSeries
                {
                    Title = lapTime.DriverId,
                    Points = { new(0,0) }
                })
                .ToDictionary(lineSeries => lineSeries.Title);

            foreach ((int lap, LapTime[] lapTimes) in lapTimesByLap)
            {
                foreach (LapTime lapTime in lapTimes)
                {
                    double relativeRaceTime = series[lapTime.DriverId].Points[^1].Y + lapTime.Time.TotalSeconds - winnerAverageTime;
                    series[lapTime.DriverId].Points.Add(new DataPoint(lap, relativeRaceTime));
                }
            }

            PlotModel plot = new();
            foreach (LineSeries lineSeries in series.Values)
            {
                lineSeries.Points.RemoveAt(0);
                plot.Series.Add(lineSeries);
            }

            return plot;
        }
    }
}
