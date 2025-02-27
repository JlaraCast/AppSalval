using AppSalval.DTOS_API;
using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskEditFormPage : ContentPage
{
	
    public RiskEditFormPage(FactorRiesgo factor)
    {
        InitializeComponent();
        BindingContext = new RiskEditViewModel(factor);
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }
}