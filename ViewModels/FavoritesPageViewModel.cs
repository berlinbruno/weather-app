namespace WeatherApp.ViewModels
{
    public partial class FavoritesPageViewModel : BaseViewModel
    {
        IConnectivity connectivity;
        ApiServices apiServices;


        public FavoritesPageViewModel(ApiServices apiServices, IConnectivity connectivity)
        {
            this.apiServices = apiServices;
            this.connectivity = connectivity;
        }

        [ObservableProperty]
        public WeatherModel weatherList = new();

        [ObservableProperty]
        public List<Hour> forecastList = new();

        [ObservableProperty]
        public Hour currentForecastList = new();

        [ObservableProperty]
        string items;


        [ObservableProperty]
        string query;


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

                    string query = Query;

                    if (query != null)
                    {

                        Preferences.Set("query", query);

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
                if (ex.Message.Contains("400"))
                {
                    await Toast.Make("Invalid City Name", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                    Debug.WriteLine(ex.Message);

                }
                else
                {
                    await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                    Debug.WriteLine(ex.Message);
                }

            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }

        [RelayCommand]
        async Task Refresh()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            try
            {
                //Check network connection.
                if (connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    Items = Preferences.Get("query", null);

                    if (Items != null)
                    {
                        Query = String.Empty;

                        //Get Api response.
                        dynamic result = await apiServices.GetWeatherBylocation(Items);
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
                if (ex.Message.Contains("400"))
                {
                    await Toast.Make("Invalid City Name", CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                    Debug.WriteLine(ex.Message);

                }
                else
                {
                    await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                    Debug.WriteLine(ex.Message);
                }
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

    }
}



