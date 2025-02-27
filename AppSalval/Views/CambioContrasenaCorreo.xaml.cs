using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class CambioContrasenaCorreo : ContentPage
    {
        private const string apiUrl = "http://savalapi.somee.com/api/Usuario"; // URL base de la API

        public CambioContrasenaCorreo()
        {
            InitializeComponent(); // Inicializa los componentes de la página
        }

        private async void OnRecoverPasswordClicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text?.Trim(); // Obtiene y limpia el texto del campo de correo

            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "Ingrese un correo válido.", "OK"); // Muestra un mensaje de error si el correo está vacío
                return;
            }

            // Verificar si el correo existe en la API
            bool emailExists = await CheckIfEmailExists(email);

            if (emailExists)
            {
                // Si el correo existe, navegar a la vista de cambio de contraseña
                await Navigation.PushAsync(new CambioContrasena(email));
            }
            else
            {
                await DisplayAlert("Error", "El correo ingresado no está registrado.", "OK"); // Muestra un mensaje de error si el correo no está registrado
            }
        }

        private async Task<bool> CheckIfEmailExists(string email)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl); // Se obtiene la lista de usuarios desde la API

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync(); // Lee la respuesta de la API
                        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonResponse); // Deserializa la respuesta en una lista de usuarios

                        // Verificar si el correo existe en la lista
                        return usuarios.Any(u => u.Correo.Equals(email, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener la lista de usuarios.", "OK"); // Muestra un mensaje de error si no se pudo obtener la lista
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "No se pudo conectar con la API.", "OK"); // Muestra un mensaje de error si no se pudo conectar con la API
                return false;
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage()); // Navega a la página de inicio de sesión
        }
    }
}
