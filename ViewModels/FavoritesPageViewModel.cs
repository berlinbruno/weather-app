namespace WeatherApp.ViewModels
{
    public partial class FavoritesPageViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public FavoritesPageViewModel(ApiServices apiServices)
        {
            this.apiServices = apiServices;
            Items = new ObservableCollection<string>();
        }

        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        string text;

        [ObservableProperty]
        public WeatherModel favoriteList = new();

        [RelayCommand]
        async Task Add()
        {

            if (string.IsNullOrWhiteSpace(Text) || Items.Count > 1 || Items.Contains(Text))
                return;

            try
            {
                dynamic result = await apiServices.GetWeatherBylocation(Text);
                FavoriteList = result;

                Preferences.Set(Items.Count.ToString(),Text);

                Items.Add(Text);
                Text = String.Empty;

                Debug.WriteLine($"{Preferences.Get("0", "no")}");
                Debug.WriteLine($"{Preferences.Get("1", "no")}");

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            }
            

        [RelayCommand]
        void Remove(string s)
        {

            if (Items.Contains(s))
            {
                string index = (Items.IndexOf(s)).ToString();

                Preferences.Remove(index);
                Items.Remove(s);
                Debug.WriteLine($"{Preferences.Get("0", "no")}");
                Debug.WriteLine($"{Preferences.Get("1", "no")}");

            }
            
        }

    }
}

