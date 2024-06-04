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
        // CA1822 will go when I add interfaces apparently
        public Dictionary<int, LapTime[]> GetLapTimes()
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new TimeSpanConverter() },
            };
            Dictionary<int, LapTime[]> lapTimesByLap = [];
            string file = "06Monaco19.json";
            string json = File.ReadAllText(file);
            JsonNode data = JsonNode.Parse(json)!;
            foreach (var lap in data!["MRData"]!["RaceTable"]!["Races"]![0]!["Laps"]!.AsArray())
            {
                lapTimesByLap[int.Parse(lap!["number"]!.GetValue<string>())] = JsonSerializer.Deserialize<LapTime[]>(lap["Timings"], options)!;
            }
            return lapTimesByLap;
        }
    }

    public record LapTime(string DriverId, int Position, TimeSpan Time);

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
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
