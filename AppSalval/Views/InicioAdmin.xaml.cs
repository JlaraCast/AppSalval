using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class InicioAdmin : ContentPage
{
    public InicioAdmin()
    {
        InitializeComponent();
        BindingContext = new InicioAdminViewModel(Navigation);
    }

    private async void OnParticipanteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GestionParticipantes());
    }

    private async void OnPreguntasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GestionPreguntas());
    }

    private async void OnRecomendacionClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecomPage());
    }

    private async void OnFactorClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RiskPage());
    }


}
