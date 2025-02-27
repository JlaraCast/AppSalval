using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class CreacionPreguntas : ContentPage
{
    public CreacionPreguntas()
    {
        InitializeComponent();
        BindingContext = new CreacionPreguntasViewModel();

        this.Appearing += OnPageAppearing;
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // Llama a un método en tu ViewModel
        var viewModel = BindingContext as CreacionPreguntasViewModel;
        viewModel?.OnPageReappearing();
    }
}