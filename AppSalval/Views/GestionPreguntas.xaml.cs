using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionPreguntas : ContentPage
{
	public GestionPreguntas()
	{
		InitializeComponent();
		BindingContext = new GestionPreguntasViewModel(Navigation);
        // Verificar si el rol del usuario es Desarrollador (rol 3)
        CanAdd = LoginPage.UserRole != "3"; // Si el rol es 3, no puede agregar
    }
<<<<<<< Updated upstream
=======

    public bool CanAdd { get; set; }

    private async void cerrrarSesion(object sender, EventArgs e)
    {
        if (LoginPage.UserRole == "3") // Si el usuario es Desarrollador (IdRol = 3)
        {
            await Navigation.PushAsync(new InicioDesarrollador());
        }
        else
        {
            await Navigation.PushAsync(new InicioAdmin());
        }
    }

>>>>>>> Stashed changes
}