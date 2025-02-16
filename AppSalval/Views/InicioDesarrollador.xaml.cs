using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class InicioDesarrollador : ContentPage
{
	public InicioDesarrollador()
	{
        InitializeComponent();
        BindingContext = new InicioDesarrolladorViewModel(Navigation);
    }
}