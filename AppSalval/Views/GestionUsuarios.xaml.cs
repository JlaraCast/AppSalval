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
        private readonly UserService _userService; // Servicio para gestionar usuarios
        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>(); // Colección observable de usuarios

        public GestionUsuarios()
        {
            InitializeComponent();
            BindingContext = this;
            _userService = new UserService();
            _ = LoadUsersAsync(); // Cargar usuarios al inicializar
            OnUserUpdated(); // Actualizar usuarios
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var usuarios = await _userService.GetUsersAsync(); // Obtener lista de usuarios
                Usuarios.Clear();

                // Ordenar usuarios según la opción seleccionada
                switch (SortPicker.SelectedIndex)
                {
                    case 0: usuarios = usuarios.OrderBy(u => u.Correo).ToList(); break;
                    case 1: usuarios = usuarios.OrderByDescending(u => u.Correo).ToList(); break;
                    case 2: usuarios = usuarios.OrderByDescending(u => u.IdUsuario).ToList(); break;
                    case 3: usuarios = usuarios.OrderBy(u => u.IdUsuario).ToList(); break;
                    case 4: usuarios = usuarios.OrderBy(u => u.RoleName).ToList(); break;
                }

                // Añadir usuarios a la colección observable
                foreach (var usuario in usuarios)
                {
                    Usuarios.Add(usuario);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar usuarios: {ex.Message}", "OK"); // Mostrar mensaje de error
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            var agregarUsuarioPage = new AgregarUsuario();
            agregarUsuarioPage.OnUserAdded += async () => await LoadUsersAsync(); // Recargar usuarios al añadir uno nuevo
            await Navigation.PushAsync(agregarUsuarioPage); // Navegar a la página de agregar usuario
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var usuario = (Usuario)button.BindingContext;

            bool isConfirmed = await DisplayAlert("Confirmación",
                "¿Estás seguro de que deseas eliminar este usuario?", "Sí", "No"); // Confirmar eliminación

            if (isConfirmed)
            {
                try
                {
                    var result = await _userService.DeleteUserAsync(usuario.IdUsuario); // Eliminar usuario

                    if (result)
                    {
                        Usuarios.Remove(usuario); // Remover usuario de la colección
                        await DisplayAlert("Éxito", "El usuario fue eliminado correctamente.", "OK"); // Mostrar mensaje de éxito
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el usuario.", "OK"); // Mostrar mensaje de error
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK"); // Mostrar mensaje de error
                }
            }
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var usuario = (Usuario)button.BindingContext;

            var editarUsuarioPage = new EditarUsuario(usuario);
            editarUsuarioPage.OnUserUpdated += async () => await LoadUsersAsync(); // Recargar usuarios al actualizar uno
            await Navigation.PushAsync(editarUsuarioPage); // Navegar a la página de editar usuario
        }

        private async void OnUserUpdated()
        {
            await LoadUsersAsync(); // Recargar usuarios
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InicioAdmin()); // Navegar a la página de inicio del administrador
        }

        private async void OnSearchUserClicked(object sender, EventArgs e)
        {
            string searchText = SearchEntry.Text;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                await DisplayAlert("Error", "Debe ingresar un id válido.", "OK"); // Mostrar mensaje de error
                return;
            }

            if (!int.TryParse(searchText, out int userId))
            {
                await DisplayAlert("Error", "El id debe ser un número válido.", "OK"); // Mostrar mensaje de error
                return;
            }

            try
            {
                var usuario = await _userService.GetUserByIdAsync(userId); // Buscar usuario por id

                if (usuario == null)
                {
                    await DisplayAlert("Aviso", "No se encuentran usuarios con el id ingresado.", "OK"); // Mostrar mensaje de aviso
                }
                else
                {
                    Usuarios.Clear();
                    Usuarios.Add(usuario); // Añadir usuario encontrado a la colección
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el usuario: {ex.Message}", "OK"); // Mostrar mensaje de error
            }
        }

        private async void OnSortOptionChanged(object sender, EventArgs e)
        {
            await LoadUsersAsync(); // Recargar usuarios al cambiar la opción de ordenamiento
        }
    }
}


