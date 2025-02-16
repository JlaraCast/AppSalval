using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomFormView : ContentPage
{
    public RecomFormView()
    {
        InitializeComponent();
        BindingContext = new RecomFormViewModel();
    }

    private void EstadoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BindingContext is RecomFormViewModel viewModel && sender is Picker picker)
        {
            viewModel.Habilitado = picker.SelectedIndex == 0; // "Activo" = true, "Inactivo" = false
        }
    }
}