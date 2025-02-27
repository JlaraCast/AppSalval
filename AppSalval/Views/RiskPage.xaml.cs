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
        await Navigation.PushAsync(new InicioDesarrollador());
    }
}