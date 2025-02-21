using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionPreguntas : ContentPage
{
	public GestionPreguntas()
	{
		InitializeComponent();
		BindingContext = new GestionPreguntasViewModel(Navigation);
    }
}