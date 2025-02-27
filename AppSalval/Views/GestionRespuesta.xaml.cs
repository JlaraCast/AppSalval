using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionRespuesta : ContentPage
{
	public GestionRespuesta()
	{
		InitializeComponent();
        BindingContext = new GestionRespuestasViewModel();
    }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioAdmin());
    }
}
