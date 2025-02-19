using AppSalval.Controllers;  // Asegúrate de tener el controlador correcto
using AppSalval.DTOS_API;       // Asegúrate de tener la clase de Usuario
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UsuarioController _usuarioController;

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
            var usuarioEncontrado = usuarioValido.FirstOrDefault(u => u.Correo == usuario && u.Contraseña == contrasena);

            if (usuarioEncontrado != null)
            {
                // Redirigir según el rol del usuario
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
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarUsuario());
        }
    }
}