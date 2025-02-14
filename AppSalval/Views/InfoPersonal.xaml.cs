using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class InfoPersonal : ContentPage
{
	public InfoPersonal()
	{
		InitializeComponent();
        BindingContext = new InfoPersonalViewModel();	
    }
}