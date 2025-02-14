namespace AppSalval.Views;

public partial class InicioAdmin : ContentPage
{
    public InicioAdmin()
    {
        InitializeComponent();

    }

    private async void CambioPaginaGestionFormulario(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GestionFormularios());
    }

    private async void CambioPaginaGestionRespuesta(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GestionRespuestas());
    }

}