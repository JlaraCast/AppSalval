using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionPreguntas : ContentPage
{
	public GestionPreguntas()
	{
		InitializeComponent();
		BindingContext = new GestionPreguntasViewModel(Navigation);
    }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioAdmin());
    }

}