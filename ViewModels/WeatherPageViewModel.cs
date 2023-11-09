using System.Globalization;

namespace WeatherApp.ViewModels
{
    public partial class WeatherPageViewModel : BaseViewModel
    {
        readonly IConnectivity connectivity;
        readonly IGeolocation geolocation;
        readonly ApiServices apiServices;

        public WeatherPageViewModel(ApiServices apiServices, IConnectivity connectivity, IGeolocation geolocation) 
        {
            this.connectivity = connectivity;
            this.geolocation = geolocation; 
            this.apiServices = apiServices;
        }

        [ObservableProperty]
        public WeatherModel weatherList = new();
        
        [ObservableProperty]
        public List<Hour> forecastList = new();

        [ObservableProperty]
        public Hour currentForecastList = new();

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool isFirstTime = true;

        

        
        [RelayCommand]
        async Task GetWeatherAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsRefreshing = true;

                //Check network connection.
                if(connectivity.NetworkAccess != NetworkAccess.Internet) 
                    return;

                // Get cached location for firsttime, else get real location.

                Double latitude;
                Double longitude;

                if (IsFirstTime == true)
                {
                    var location = await geolocation.GetLastKnownLocationAsync();

                    IsFirstTime = false;

                    if (location == null)
                        return;

                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }
                else
                {
                    var location = await geolocation.GetLocationAsync(new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(10)    
                        });
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }

                //Get Api response.
                dynamic result = await apiServices.GetWeatherBylocation(latitude, longitude);
                WeatherList = result;

                //Join Hours of 2 days from now.
                var weatherForecastHours = new List<Hour>(WeatherList.Forecast.Forecastday[0].Hour);
                weatherForecastHours.AddRange(new List<Hour>(WeatherList.Forecast.Forecastday[1].Hour));

                //Find index of next Hour.
                int nextIndexHour = int.Parse(DateTime.Now.ToString("HH", CultureInfo.CurrentCulture));
                weatherForecastHours.ForEach(x => x.Time = x.Time[11..]);

                //Get next 24 Hours from now.
                ForecastList = weatherForecastHours.GetRange(nextIndexHour + 1, 12);

                CurrentForecastList = weatherForecastHours[nextIndexHour];

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
