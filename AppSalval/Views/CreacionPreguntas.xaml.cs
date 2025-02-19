using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class CreacionPreguntas : ContentPage
{
    public CreacionPreguntas()
    {
        InitializeComponent();
        BindingContext = new CreacionPreguntasViewModel();
    }
}