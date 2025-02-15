using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


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
            var button = (Button)sender;
            var usuario = (Usuario)button.BindingContext;

            // Mostrar pop-up de confirmación
            bool isConfirmed = await DisplayAlert("Confirmación",
                "¿Estás seguro de que deseas eliminar este usuario?",
                "Sí",
                "No");

            if (isConfirmed)
            {
                // Llamar al servicio para eliminar el usuario
                var result = await _userService.DeleteUserAsync(usuario.IdUsuario);

                if (result)
                {
                    Usuarios.Remove(usuario);  // Actualizamos la lista
                    await DisplayAlert("Éxito", "El usuario fue eliminado correctamente.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar el usuario. Intenta nuevamente.", "OK");
                }
            }
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var usuario = (Usuario)button.BindingContext;

            var editarUsuarioPage = new EditarUsuario(usuario);
            editarUsuarioPage.OnUserUpdated += async () => await LoadUsersAsync();
            await Navigation.PushAsync(editarUsuarioPage);
        }


        // Método para actualizar la lista de usuarios después de la edición
        private async void OnUserUpdated()
        {
            await LoadUsersAsync();
        }
    }
}


