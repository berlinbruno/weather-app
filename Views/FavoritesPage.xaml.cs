namespace WeatherApp.Views;

public partial class FavoritesPage : ContentPage
{

	public FavoritesPage(FavoritesPageViewModel favoritesPageViewModel)
	{
		InitializeComponent();
		BindingContext = favoritesPageViewModel;
	}
}