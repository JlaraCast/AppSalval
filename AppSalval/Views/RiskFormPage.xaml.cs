using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskFormPage : ContentPage
{
	public RiskFormPage()
	{
		InitializeComponent();
        BindingContext = new RiskFormViewModel();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }

    private void EstadoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BindingContext is RiskFormViewModel viewModel && sender is Picker picker)
        {
            viewModel.Habilitado = picker.SelectedIndex == 0; // "Activo" = true, "Inactivo" = false
        }
    }
}