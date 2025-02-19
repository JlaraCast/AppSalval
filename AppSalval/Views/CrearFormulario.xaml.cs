using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class CrearFormulario : ContentPage
{
    public CrearFormulario()
    {
        InitializeComponent();
        BindingContext = new CrearFormularioViewModel();
    }

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        if (checkBox?.BindingContext is PreguntaViewModel pregunta)
        {
            var viewModel = BindingContext as CrearFormularioViewModel;
            viewModel?.ActualizarPreguntasSeleccionadas();
        }
    }
}


