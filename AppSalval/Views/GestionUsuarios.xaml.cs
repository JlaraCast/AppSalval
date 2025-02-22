using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class GestionUsuarios : ContentPage
    {
        private readonly UserService _userService;
        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>();

        public GestionUsuarios()
        {
            InitializeComponent();
            BindingContext = this;
            _userService = new UserService();
            _ = LoadUsersAsync();
            OnUserUpdated();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var usuarios = await _userService.GetUsersAsync();
                Usuarios.Clear();

                switch (SortPicker.SelectedIndex)
                {
                    case 0: usuarios = usuarios.OrderBy(u => u.Correo).ToList(); break;
                    case 1: usuarios = usuarios.OrderByDescending(u => u.Correo).ToList(); break;
                    case 2: usuarios = usuarios.OrderByDescending(u => u.IdUsuario).ToList(); break;
                    case 3: usuarios = usuarios.OrderBy(u => u.IdUsuario).ToList(); break;
                    case 4: usuarios = usuarios.OrderBy(u => u.RoleName).ToList(); break;
                }

                foreach (var usuario in usuarios)
                {
                    Usuarios.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar usuarios: {ex.Message}", "OK");
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var agregarUsuarioPage = new AgregarUsuario();
            agregarUsuarioPage.OnUserAdded += async () => await LoadUsersAsync();
            await Navigation.PushAsync(agregarUsuarioPage);
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var usuario = (Usuario)button.BindingContext;

            bool isConfirmed = await DisplayAlert("Confirmación",
                "¿Estás seguro de que deseas eliminar este usuario?", "Sí", "No");

            if (isConfirmed)
            {
                try
                {
                    var result = await _userService.DeleteUserAsync(usuario.IdUsuario);

                    if (result)
                    {
                        Usuarios.Remove(usuario);
                        await DisplayAlert("Éxito", "El usuario fue eliminado correctamente.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el usuario.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var usuario = (Usuario)button.BindingContext;

            var editarUsuarioPage = new EditarUsuario(usuario);
            editarUsuarioPage.OnUserUpdated += async () => await LoadUsersAsync();
            await Navigation.PushAsync(editarUsuarioPage);
        }

        private async void OnUserUpdated()
        {
            await LoadUsersAsync();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InicioAdmin());
        }

        private async void OnSearchUserClicked(object sender, EventArgs e)
        {
            string searchText = SearchEntry.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                await DisplayAlert("Error", "Debe ingresar un id válido.", "OK");
                return;
            }

            if (!int.TryParse(searchText, out int userId))
            {
                await DisplayAlert("Error", "El id debe ser un número válido.", "OK");
                return;
            }

            try
            {
                var usuario = await _userService.GetUserByIdAsync(userId);

                if (usuario == null)
                {
                    await DisplayAlert("Aviso", "No se encuentran usuarios con el id ingresado.", "OK");
                }
                else
                {
                    Usuarios.Clear();
                    Usuarios.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el usuario: {ex.Message}", "OK");
            }
        }

        private async void OnSortOptionChanged(object sender, EventArgs e)
        {
            await LoadUsersAsync();
        }
    }
}


