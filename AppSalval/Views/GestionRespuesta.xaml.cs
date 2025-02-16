using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionRespuesta : ContentPage
{
	public GestionRespuesta()
	{
		InitializeComponent();
        BindingContext = new GestionRespuestasViewModel();
    }
}
