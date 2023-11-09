namespace WeatherApp.Views;

//[XamlCompilation(XamlCompilationOptions.Skip)]

public partial class WeatherPage : ContentPage
{
    public WeatherPage(WeatherPageViewModel weatherPageViewModel)
    {
        InitializeComponent();
        BindingContext = weatherPageViewModel;

        weatherPageViewModel.GetWeatherCommand.Execute(this);

    }

        
}