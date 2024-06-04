using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRaceTrace
{
    public class ErgastService
    {
        // CA1822 will go when I add interfaces apparently
        public Dictionary<int, LapTime[]> GetLapTimes()
        {
            Dictionary<int, LapTime[]> lapTimesByLap = [];
            string file = "06Monaco19.json";
            string json = File.ReadAllText(file);
            dynamic? data = JsonConvert.DeserializeObject(json);
            foreach (var lap in data!.MRData.RaceTable.Races[0].Laps)
            {
                lapTimesByLap[lap.Value<int>("number")] = lap.Timings.ToObject<LapTime[]>();
            }
            return lapTimesByLap;
        }
    }

    public record LapTime(string DriverId, int Position, TimeSpan Time)
    {
        [JsonConstructor]
        public LapTime(string DriverId, int Position, string Time) : 
            this(DriverId, Position, TimeSpan.ParseExact(Time, @"%m\:ss\.fff", CultureInfo.InvariantCulture)) { }
    }
}
