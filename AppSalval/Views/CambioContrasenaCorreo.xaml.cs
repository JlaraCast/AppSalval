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
            InitializeComponent();
        }

        private async void OnRecoverPasswordClicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text?.Trim();

            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "Ingrese un correo válido.", "OK");
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
                await DisplayAlert("Error", "El correo ingresado no está registrado.", "OK");
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
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(jsonResponse);

                        // Verificar si el correo existe en la lista
                        return usuarios.Any(u => u.Correo.Equals(email, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener la lista de usuarios.", "OK");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "No se pudo conectar con la API.", "OK");
                return false;
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
