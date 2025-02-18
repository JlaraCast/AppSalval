using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace AppSalval.Views
{
    public partial class AgregarUsuario : ContentPage
    {
        public ObservableCollection<Usuario> Usuarios { get; set; } // Definir la propiedad Usuarios

        public event Action OnUserAdded; // Evento para actualizar la lista

        public AgregarUsuario()
        {
            InitializeComponent();
            Usuarios = new ObservableCollection<Usuario>(); // Inicializar la colección
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();  // Regresar a la vista anterior
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            await AddUserToApiAsync();
        }

        private async Task AddUserToApiAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(EntryEmail.Text) || PickerRole.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                    return;
                }

                var newUser = new Usuario
                {
                    Correo = EntryEmail.Text.Trim(),
                    Contraseña = "123456",  // Puedes permitir que el usuario ingrese su contraseña
                    IdRol = GetRoleId(PickerRole.SelectedItem.ToString())
                };

                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://savalapi.somee.com/api/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Usuario", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Usuario agregado correctamente", "OK");
                    OnUserAdded?.Invoke();  // Notificar actualización
                    await Navigation.PopAsync();
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo agregar el usuario: {errorMessage}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al conectar con la API: {ex.Message}", "OK");
            }
        }

        private int GetRoleId(string roleName)
        {
            return roleName switch
            {
                "Administrador" => 1,
                "Encuestador" => 2,
                "Desarrollador" => 3,
                _ => 0
            };
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Usuario usuario)
            {
                bool confirm = await DisplayAlert("Confirmar eliminación",
                                                  $"¿Estás seguro de que deseas eliminar a {usuario.Correo}?",
                                                  "Sí", "Cancelar");

                if (!confirm) return;

                try
                {
                    using var client = new HttpClient();
                    client.BaseAddress = new Uri("http://savalapi.somee.com/api/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.DeleteAsync($"Usuario/{usuario.IdUsuario}");

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Usuario eliminado correctamente.", "OK");

                        // Eliminar el usuario de la lista localmente
                        var usuariosList = Usuarios.ToList();
                        usuariosList.Remove(usuario);
                        Usuarios = new ObservableCollection<Usuario>(usuariosList);
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", $"No se pudo eliminar el usuario: {errorMessage}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al conectar con la API: {ex.Message}", "OK");
                }
            }
        }
    }
}
