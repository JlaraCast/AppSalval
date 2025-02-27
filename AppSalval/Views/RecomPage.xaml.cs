using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomPage : ContentPage
{
    public RecomPage()
    {
        InitializeComponent();
        BindingContext = new RecomViewModel(Navigation);
    }

    private async void volver(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InicioDesarrollador());
    }
}