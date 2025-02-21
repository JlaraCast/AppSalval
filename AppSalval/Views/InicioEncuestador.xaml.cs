namespace AppSalval.Views;

public partial class InicioEncuestador : ContentPage
{
    public InicioEncuestador()
    {
        InitializeComponent();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navega a la p�gina de login
        await Navigation.PushAsync(new LoginPage());
    }

    private async void OnFormularioClicked(object sender, EventArgs e)
    {
        // Navega a la p�gina de encuestas
        await Navigation.PushAsync(new Formularios());
    }
}