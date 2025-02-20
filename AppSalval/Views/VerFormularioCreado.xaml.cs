using AppSalval.Models_Api;
using AppSalval.ViewModels;


namespace AppSalval.Views;

public partial class VerFormularioCreado : ContentPage
{
	public VerFormularioCreado(FormularioDto formulario)
	{
		InitializeComponent();
		BindingContext = new VerFormularioCreadoViewModel(Navigation, formulario, ListaPreguntas);
    }

}