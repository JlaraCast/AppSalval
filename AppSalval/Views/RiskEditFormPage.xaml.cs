using AppSalval.DTOS_API;
using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RiskEditFormPage : ContentPage
{
	
    public RiskEditFormPage(FactorRiesgo factor)
    {
        InitializeComponent();
        BindingContext = new RiskEditViewModel(factor);
    }
}