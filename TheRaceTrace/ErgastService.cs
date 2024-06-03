using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRaceTrace
{
    internal class ErgastService
    {
        // CA1822 will go when I add interfaces apparently
        //public Dictionary<int, List<LapTime>> GetLapTimes()
        public void GetLapTimes()
        {
            using (StreamReader r = new("06Monaco19.json"))
            {
                string json = r.ReadToEnd();
                dynamic? data = JsonConvert.DeserializeObject(json);
            }
        }
    }

    internal record LapTime(string DriverId, int Position, TimeSpan Time);
}
