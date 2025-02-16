using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskFormPage : ContentPage
{
	public RiskFormPage()
	{
		InitializeComponent();
        BindingContext = new RiskFormViewModel();
    }

    private void EstadoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BindingContext is RiskFormViewModel viewModel && sender is Picker picker)
        {
            viewModel.Habilitado = picker.SelectedIndex == 0; // "Activo" = true, "Inactivo" = false
        }
    }
}