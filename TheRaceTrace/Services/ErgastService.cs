using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TheRaceTrace.Services
{
    public interface IErgastService
    {
        RaceData GetRaceData();
    }

    public class ErgastService : IErgastService
    {
        // TODO: Maybe validate the data (eg no missing laps etc)
        public RaceData GetRaceData()
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Converters = { new TimeSpanConverter() },
            };
            SortedDictionary<int, LapTime[]> lapTimesByLap = [];

            HttpClient client = new();
            // TODO: Maybe this should be another service/method so it can be mocked?
            HttpResponseMessage response = client.GetAsync("http://ergast.com/api/f1/current/last/laps.json?limit=2000").Result;
            String json;
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Custom, more descriptive exception
                //throw new Exception("Ergast is unhappy");
                json = File.ReadAllText("fallback_data.json");
            }
            else
            {
                json = response.Content.ReadAsStringAsync().Result;
            }
            // TODO: Check the json structure is as expected
            JsonNode race = JsonNode.Parse(json)!["MRData"]!["RaceTable"]!["Races"]![0]!;
            foreach (JsonNode? lap in race["Laps"]!.AsArray())
            {
                lapTimesByLap[int.Parse(lap!["number"]!.GetValue<string>())] = lap["Timings"].Deserialize<LapTime[]>(options)!;
            }
            return new RaceData($"{race["season"]!.GetValue<string>()} {race["raceName"]!.GetValue<string>()}", lapTimesByLap);
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
