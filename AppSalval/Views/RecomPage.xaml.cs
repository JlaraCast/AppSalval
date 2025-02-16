using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomPage : ContentPage
{
    public RecomPage()
    {
        InitializeComponent();
        BindingContext = new RecomViewModel(Navigation);
    }
}