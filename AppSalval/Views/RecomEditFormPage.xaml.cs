using AppSalval.DTOS_API;
using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomEditFormPage : ContentPage
{
    public RecomEditFormPage(Recomendacion recomendacion)
    {
        InitializeComponent();
        BindingContext = new RecomEditViewModel(recomendacion);
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }
}