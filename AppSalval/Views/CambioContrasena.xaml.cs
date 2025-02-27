using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Linq;

namespace AppSalval.Views
{
    public partial class CambioContrasena : ContentPage
    {
        private const string ApiUrl = "http://savalapi.somee.com/api/Usuario";
        private readonly string _correo; // Variable para almacenar el correo

        public CambioContrasena(string correo)
        {
            InitializeComponent();
            _correo = correo; // Guardamos el correo recibido
        }

        private async void OnSavePasswordClicked(object sender, EventArgs e)
        {
            string nuevaContrasena = txtNuevaContrasena.Text;
            string confirmarContrasena = txtConfirmarContrasena.Text;

            // Validar campos
            if (string.IsNullOrEmpty(nuevaContrasena) || string.IsNullOrEmpty(confirmarContrasena))
            {
                await DisplayAlert("Error", "Por favor complete ambos campos.", "OK");
                return;
            }

            if (nuevaContrasena != confirmarContrasena)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            if (nuevaContrasena.Length < 8)
            {
                await DisplayAlert("Error", "La contraseña debe tener al menos 8 caracteres.", "OK");
                return;
            }

            // Obtener usuario por correo
            var usuario = await ObtenerUsuarioPorCorreo(_correo);

            if (usuario == null)
            {
                await DisplayAlert("Error", "Usuario no encontrado.", "OK");
                return;
            }

            // Actualizar contraseña
            usuario.Contraseña = nuevaContrasena;
            bool exito = await ActualizarContrasena(usuario);

            if (exito)
            {
                await DisplayAlert("Éxito", "La contraseña ha sido cambiada.", "OK");
                await Navigation.PushAsync(new LoginPage()); // Regresar a la vista anterior
            }
            else
            {
                await DisplayAlert("Error", "No se pudo cambiar la contraseña.", "OK");
            }
        }

        private async Task<Usuario> ObtenerUsuarioPorCorreo(string correo)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{ApiUrl}?correo={correo}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content);

            return usuarios?.FirstOrDefault(u => u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> ActualizarContrasena(Usuario usuario)
        {
            using var client = new HttpClient();
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{ApiUrl}/{usuario.IdUsuario}", content);

            return response.IsSuccessStatusCode;
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea cancelar?", "Sí", "No");

            if (confirm)
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }
    }
}


