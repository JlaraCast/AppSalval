using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class CrearFormulario : ContentPage
{
	public CrearFormulario()
	{
		InitializeComponent();
        BindingContext = new CrearFormularioViewModel(Navigation);

    }
}