using System.Net.Http.Headers;
using System.Text.Json;

namespace AppSalval.Views
{
    public partial class EditarUsuario : ContentPage
    {
        public Usuario Usuario { get; set; }
        public event Action OnUserUpdated;

        public EditarUsuario(Usuario usuario)
        {
            InitializeComponent();

            Usuario = usuario;
            BindingContext = this;

            EntryEmail.Text = Usuario.Correo;
            EntryPassword.Text = Usuario.Contraseña;
            PickerRole.SelectedIndex = Usuario.IdRol - 1;
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntryEmail.Text) || string.IsNullOrEmpty(EntryPassword.Text))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos.", "OK");
                return;
            }

            string newEmail = EntryEmail.Text.Trim();
            string oldEmail = Usuario.Correo;

            // Si el usuario cambió su correo, verificar si ya existe en la base de datos
            if (!newEmail.Equals(oldEmail, StringComparison.OrdinalIgnoreCase) && await IsEmailRegisteredAsync(newEmail))
            {
                await DisplayAlert("Error", "El correo electrónico ya está en uso por otro usuario.", "OK");
                return;
            }

            Usuario.Correo = newEmail;
            Usuario.Contraseña = EntryPassword.Text.Trim();

            switch (PickerRole.SelectedIndex)
            {
                case 0: Usuario.IdRol = 1; break;
                case 1: Usuario.IdRol = 2; break;
                case 2: Usuario.IdRol = 3; break;
                default: Usuario.IdRol = 0; break;
            }

            var userService = new UserService();
            var result = await userService.UpdateUserAsync(Usuario);

            if (result)
            {
                await DisplayAlert("Éxito", "Los cambios fueron guardados correctamente.", "OK");
                OnUserUpdated?.Invoke();
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar los cambios. Intenta nuevamente.", "OK");
            }
        }

        private async Task<bool> IsEmailRegisteredAsync(string email)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://savalapi.somee.com/api/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("Usuario"); // Obtener todos los usuarios

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Verificar si algún usuario tiene el mismo correo (excepto el usuario actual)
                    return usuarios.Any(u => u.Correo.Equals(email, StringComparison.OrdinalIgnoreCase) && u.IdUsuario != Usuario.IdUsuario);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo verificar el correo: {ex.Message}", "OK");
            }

            return false;
        }


        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
