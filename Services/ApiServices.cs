using Newtonsoft.Json;

namespace WeatherApp.Services
{
    public class ApiServices
    {
        readonly HttpClient httpClient;
        public ApiServices()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<WeatherModel>GetWeatherBylocation(Double latitude, Double longitude)
        {

            
            dynamic response = await httpClient.GetStringAsync(string.Format("https://api.weatherapi.com/v1/forecast.json?key=306f4d66aa5f47a3816170109230208&q={0},{1}&days=2&aqi=no&alerts=no",latitude,longitude));
            
            return JsonConvert.DeserializeObject<WeatherModel>(response);
        }
    }
}
