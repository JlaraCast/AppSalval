using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class CreacionPreguntas : ContentPage
{
    public CreacionPreguntas()
    {
        InitializeComponent();
        BindingContext = new CreacionPreguntasViewModel();

        // Suscribe el evento Appearing a un manejador de eventos
        this.Appearing += OnPageAppearing;
    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // Llama a un m�todo en tu ViewModel cuando la p�gina aparece
        var viewModel = BindingContext as CreacionPreguntasViewModel;
        viewModel?.OnPageReappearing();
    }
}
