using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskPage : ContentPage
{
    public RiskPage()
    {
        InitializeComponent();
        BindingContext = new RiskViewModel(Navigation);
    }
}