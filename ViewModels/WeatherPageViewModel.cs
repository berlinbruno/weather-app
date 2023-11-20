
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
                if (connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    string query = await GetLocationAsync();

                    if (query != null)
                    {

                        //Get Api response.
                        dynamic result = await apiServices.GetWeatherBylocation(query);
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

                        if (CurrentForecastList.IsDay == 0)
                        {
                            IsDayOrNight = "n";
                        }
                        else
                        {
                            IsDayOrNight = "d";
                        }

                    }
                    else
                    {
                        Debug.WriteLine("Invalid Query");
                        return;
                    }

                }
                else
                {
                    await Toast.Make("No Internet", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                    Debug.WriteLine("No Internet");
                    return;
                }

            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }

        async Task<string> GetLocationAsync()
        {
            try
            {
                Double latitude;
                Double longitude;
                string query;

                // Get cached location for firsttime, else get real location.
                if (IsFirstTime == true)
                {
                    var location = await geolocation.GetLastKnownLocationAsync();

                    IsFirstTime = false;

                    if (location == null)
                        location = await geolocation.GetLocationAsync(new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(10)
                        });

                    latitude = location.Latitude;
                    longitude = location.Longitude;

                    query = string.Format("{0},{1}", location.Latitude, location.Longitude);
                    return query;
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
                    query = string.Format("{0},{1}", location.Latitude, location.Longitude);
                    return query;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Toast.Make(fnsEx.Message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                Debug.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await Toast.Make("Location Not Enabled", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                Debug.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                await Toast.Make("No Location Access", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                Debug.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                Debug.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
