using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace TheRaceTrace.Services
{
    public interface IChartService
    {
        PlotModel CreateTrace(RaceData raceData);
    }

    public class ChartService : IChartService
    {
        public PlotModel CreateTrace(RaceData raceData)
        {
            SortedDictionary<int, LapTime[]> lapTimesByLap = raceData.LapTimesByLap;
            int lapCount = lapTimesByLap.Count;
            string winner = lapTimesByLap[lapCount]
                .MinBy(lapTime => lapTime.Position)!
                .DriverId;

            double winnerAverageTime = lapTimesByLap.Values
                .SelectMany(lapTimes => lapTimes)
                .Where(lapTime => lapTime.DriverId == winner)
                .Select(lapTime => lapTime.Time.TotalSeconds)
                .Average();

            Dictionary<string, LineSeries> seriesByDriver = lapTimesByLap[1]
                .Select(lapTime => new LineSeries
                {
                    Title = lapTime.DriverId,
                    Points = { new(0, 0) }
                })
                .ToDictionary(lineSeries => lineSeries.Title);

            foreach ((int lap, LapTime[] lapTimes) in lapTimesByLap)
            {
                foreach (LapTime lapTime in lapTimes)
                {
                    double relativeRaceTime = seriesByDriver[lapTime.DriverId].Points[^1].Y - lapTime.Time.TotalSeconds + winnerAverageTime;
                    seriesByDriver[lapTime.DriverId].Points.Add(new DataPoint(lap, relativeRaceTime));
                }
            }

            PlotModel plot = new();
            foreach (LineSeries lineSeries in seriesByDriver.Values)
            {
                lineSeries.Points.RemoveAt(0);
                lineSeries.CanTrackerInterpolatePoints = false;
                lineSeries.TrackerFormatString = "Lap {2}: {4:0.###} {0}";
                plot.Series.Add(lineSeries);
            }
            plot.Title = raceData.RaceName;
            plot.Legends.Add(new Legend { LegendPlacement = LegendPlacement.Outside });

            return plot;
        }
    }
}
