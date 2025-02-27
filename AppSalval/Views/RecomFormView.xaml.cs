using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomFormView : ContentPage
{
    public RecomFormView()
    {
        InitializeComponent();
        BindingContext = new RecomFormViewModel();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();  // Regresar a la vista anterior
    }

    private void EstadoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BindingContext is RecomFormViewModel viewModel && sender is Picker picker)
        {
            viewModel.Habilitado = picker.SelectedIndex == 0; // "Activo" = true, "Inactivo" = false
        }
    }
}