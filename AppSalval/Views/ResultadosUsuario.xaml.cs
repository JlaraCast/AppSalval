using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class ResultadosUsuario : ContentPage
{
	public ResultadosUsuario()
	{
		InitializeComponent();
        BindingContext = new ResultadosUsuarioViewModel();
    }
}