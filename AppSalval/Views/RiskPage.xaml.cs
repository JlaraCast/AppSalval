using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskPage : ContentPage
{
    public RiskPage()
    {
        InitializeComponent();
        BindingContext = new RiskViewModel(Navigation);
    }

    private async void volver(object sender, EventArgs e)
    {
        if (LoginPage.UserRole == "3") // Si el usuario es Desarrollador (IdRol = 3)
        {
            await Navigation.PushAsync(new InicioDesarrollador());
        }
        else
        {
            await Navigation.PushAsync(new InicioAdmin());
        }
    }
}