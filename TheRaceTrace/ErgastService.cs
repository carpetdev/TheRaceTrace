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
        //public static Dictionary<int, List<LapTime>> GetLapTimes()
        public static void GetLapTimes()
        {
            using (StreamReader r = new StreamReader("06Monaco19.json"))
            {
                string json = r.ReadToEnd();
                dynamic? data = JsonConvert.DeserializeObject(json);
                Debug.WriteLine(data);
            }
        }
    }

    internal record LapTime(string DriverId, int Position, TimeSpan Time);
}
