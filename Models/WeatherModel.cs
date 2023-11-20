// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using Newtonsoft.Json;
using System.Globalization;

namespace WeatherApp.Models
{

    public class Astro
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }
    }

    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        public string NewIcon => Icon.Replace("68x68","128x128");

        public string NewIcon1 => Icon.Substring(Icon.Length - 7, 7);
    }

    public class Current
    {
        [JsonProperty("temp_c")]
        public double TempC { get; set; }

        [JsonProperty("temp_f")]
        public double TempF { get; set; }

        [JsonProperty("is_day")]
        public int IsDay { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("wind_mph")]
        public double WindMph { get; set; }

        [JsonProperty("wind_kph")]
        public double WindKph { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }

    public class Forecastday
    {
        [JsonProperty("astro")]
        public Astro Astro { get; set; }

        [JsonProperty("hour")]
        public List<Hour> Hour { get; set; }
    }

    public class Hour
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        public string FormattedTime
        {
            get
            {
                if (DateTime.TryParse(Time, out DateTime timeValue))
                {
                    return timeValue.ToString("hh:mm", CultureInfo.InvariantCulture);
                }
                return "Invalid Time"; // Or any default value indicating an issue with the provided time string
            }
        }

        [JsonProperty("temp_c")]
        public double TempC { get; set; }

        public double TemperatureC => Math.Round(TempC);

        [JsonProperty("temp_f")]
        public double TempF { get; set; }

        [JsonProperty("is_day")]
        public int IsDay { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("will_it_rain")]
        public int WillItRain { get; set; }

        [JsonProperty("chance_of_rain")]
        public int ChanceOfRain { get; set; }
    }

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("tz_id")]
        public string TzId { get; set; }

        [JsonProperty("localtime_epoch")]
        public int LocaltimeEpoch { get; set; }

        [JsonProperty("localtime")]
        public string Localtime { get; set; }

        public string NewLocalTime => Localtime.Substring(Localtime.Length - 5, 2);
    }

    public class WeatherModel
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }

        [JsonProperty("forecast")]
        public Forecast Forecast { get; set; }
    }




}
