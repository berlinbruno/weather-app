using System.Globalization;

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
        public List<Hour> forecastList = new();

        [ObservableProperty]
        public Hour currentForecastList = new();

        [ObservableProperty]
        string items;


        [ObservableProperty]
        string query;

        [ObservableProperty]
        public WeatherModel favoriteList = new();


        [RelayCommand]
        async Task Add()
        {

            if (IsBusy)
                return;

            try
            {

                IsBusy = true;
                IsRefreshing = true;

                //Check network connection.
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                    return;

                Debug.WriteLine("a");

                if (Query == null)
                    return;

               
                    Debug.WriteLine("a1");
                    Preferences.Clear("query");
                    Debug.WriteLine("a2");
                    Preferences.Set("query", Query);
                    Items = Preferences.Get("query", null);
             

                Debug.WriteLine("b");

                Query = String.Empty;

                //Get Api response.
                dynamic result = await apiServices.GetWeatherBylocation(Items);
                FavoriteList = result;
                Debug.WriteLine("c");

                //Join Hours of 2 days from now.
                var weatherForecastHours = new List<Hour>(FavoriteList.Forecast.Forecastday[0].Hour);
                weatherForecastHours.AddRange(new List<Hour>(FavoriteList.Forecast.Forecastday[1].Hour));

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

        [RelayCommand]
        async Task Refresh()
        {
            if (IsBusy)
                return;

            try
            {
                Items = Preferences.Get("query", null);

                if (Items == null)
                    return;

                Query = String.Empty;

                //Get Api response.
                dynamic result = await apiServices.GetWeatherBylocation(Items);
                FavoriteList = result;

                //Join Hours of 2 days from now.
                var weatherForecastHours = new List<Hour>(FavoriteList.Forecast.Forecastday[0].Hour);
                weatherForecastHours.AddRange(new List<Hour>(FavoriteList.Forecast.Forecastday[1].Hour));

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

        

