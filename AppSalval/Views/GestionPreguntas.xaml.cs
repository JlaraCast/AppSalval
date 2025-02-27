using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionPreguntas : ContentPage
{
	public GestionPreguntas()
	{
		InitializeComponent();
		BindingContext = new GestionPreguntasViewModel(Navigation);
        this.Appearing += OnPageAppearing;
    }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioAdmin());
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // Llama a un método en tu ViewModel
        var viewModel = BindingContext as GestionPreguntasViewModel;
    }

}