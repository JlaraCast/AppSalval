using AppSalval.Models_Api;
using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class EditarFormulario : ContentPage
{
	public EditarFormulario(FormularioDto formulario)
	{
		InitializeComponent();
        BindingContext = new EditarFormularioViewModel(Navigation, formulario);
    }
}