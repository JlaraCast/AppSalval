using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionPreguntasDesarrollador : ContentPage
{
	public GestionPreguntasDesarrollador()
	{
        InitializeComponent();
        BindingContext = new GestionPreguntasViewModel(Navigation);
        this.Appearing += OnPageAppearing;
    }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioDesarrollador());
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // Llama a un método en tu ViewModel
        var viewModel = BindingContext as GestionPreguntasViewModel;
    }
}