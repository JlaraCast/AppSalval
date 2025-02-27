using AppSalval.Controllers;  // Aseg�rate de tener el controlador correcto
using AppSalval.DTOS_API;       // Aseg�rate de tener la clase de Usuario
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UsuarioController _usuarioController;
        public static string UserRole { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            _usuarioController = new UsuarioController();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contrasena = txtContrasena.Text;

            // Llamamos al controlador para validar las credenciales contra la API
            var usuarioValido = await _usuarioController.GetUsuariosAsync();

            // Buscamos el usuario en la lista obtenida de la API
            var usuarioEncontrado = usuarioValido.FirstOrDefault(u => u.Correo == usuario && u.Contrase�a == contrasena);

            if (usuarioEncontrado != null)
            {
                UserRole = usuarioEncontrado.IdRol.ToString();
                // Redirigir seg�n el rol del usuario
                switch (usuarioEncontrado.IdRol)
                {
                    case 1: // Administrador
                        await Navigation.PushAsync(new InicioAdmin());
                        break;
                    case 2: // Encuestador 
                        await Navigation.PushAsync(new InicioEncuestador());
                        break;

                    case 3: // Desarrollador
                        await Navigation.PushAsync(new InicioDesarrollador());
                        break;

                    default:
                        await DisplayAlert("Error", "Rol desconocido", "OK");
                        break;
                }
            }
            else
            {
                // Si no se encuentra el usuario o las credenciales no son correctas
                await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
            }
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CambioContrasenaCorreo());
        }

        private async void OnCloseAppButtonClicked(object sender, EventArgs e)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Confirmaci�n",
            "�Est�s seguro de que quieres salir de la aplicaci�n?",
            "S�",
            "No"
            );

            if (confirm && Application.Current is not null)
            {
                Application.Current.Quit();
            }
        }

    }
}