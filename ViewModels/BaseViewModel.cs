namespace WeatherApp.ViewModels
{
    public partial class BaseViewModel:ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string isDayOrNight;

        public bool IsNotBusy => !IsBusy;

    }
}
