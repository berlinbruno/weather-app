namespace WeatherApp.ViewModels
{
    public partial class FavoritesPageViewModel : BaseViewModel
    {
        public FavoritesPageViewModel()
        {
            Items = new ObservableCollection<string>();
        }

        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            Items.Add(Text);
            Text = String.Empty;
        }

        [RelayCommand]
        void Remove(string s)
        {
            if (Items.Contains(s))
            {
                Items.Remove(s);
            }
        }
    }
}

