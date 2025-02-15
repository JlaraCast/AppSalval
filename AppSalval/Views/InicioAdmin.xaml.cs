using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class InicioAdmin : ContentPage
{
    public InicioAdmin()
    {
        InitializeComponent();
        BindingContext = new InicioAdminViewModel(Navigation);
    }

    
}
