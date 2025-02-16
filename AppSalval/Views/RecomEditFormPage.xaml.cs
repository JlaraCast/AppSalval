using AppSalval.DTOS_API;
using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class RecomEditFormPage : ContentPage
{
    public RecomEditFormPage(Recomendacion recomendacion)
    {
        InitializeComponent();
        BindingContext = new RecomEditViewModel(recomendacion);
    }
}