using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class InicioDesarrollador : ContentPage
{
	public InicioDesarrollador()
	{
        InitializeComponent();
        BindingContext = new InicioDesarrolladorViewModel(Navigation);
    }
    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}