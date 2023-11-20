namespace WeatherApp.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private void GetStartedButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new AppShell());
    }
}