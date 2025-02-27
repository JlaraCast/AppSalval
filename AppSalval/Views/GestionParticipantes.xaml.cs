using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionParticipantes : ContentPage
{
    public GestionParticipantes()
    {
        InitializeComponent();
        BindingContext = new GestionParticipantesViewModel();
    }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioAdmin());
    }
}