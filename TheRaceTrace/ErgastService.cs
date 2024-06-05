using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TheRaceTrace
{
    public class ErgastService
    {
        // TODO CA1822 will go when I add interfaces apparently
        // TODO Maybe validate the data (eg no missing laps etc)
        public SortedDictionary<int, LapTime[]> GetLapTimes()
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new TimeSpanConverter() },
            };
            SortedDictionary<int, LapTime[]> lapTimesByLap = [];
            string file = "06Monaco19.json";
            string json = File.ReadAllText(file);
            JsonNode data = JsonNode.Parse(json)!;
            foreach (JsonNode? lap in data!["MRData"]!["RaceTable"]!["Races"]![0]!["Laps"]!.AsArray())
            {
                lapTimesByLap[int.Parse(lap!["number"]!.GetValue<string>())] = JsonSerializer.Deserialize<LapTime[]>(lap["Timings"], options)!;
            }
            return lapTimesByLap;
        }
    }

    public record LapTime(string DriverId, int Position, TimeSpan Time);

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        // TODO: What do when not in this format (lap time over an hour, ergast mistake, etc.)
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.ParseExact(reader.GetString()!, @"%m\:ss\.fff", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"%m\:ss\.fff", CultureInfo.InvariantCulture));
        }
    }
}
