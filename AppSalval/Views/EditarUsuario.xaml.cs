using System;


namespace AppSalval.Views
{
    public partial class EditarUsuario : ContentPage
    {
        public Usuario Usuario { get; set; }
        public event Action OnUserUpdated;

        public EditarUsuario(Usuario usuario)
        {
            InitializeComponent();

            // Asignar el usuario a la propiedad y llenar los controles con sus datos
            Usuario = usuario;
            BindingContext = this;

            EntryPassword.Text = Usuario.Contraseña;
            PickerRole.SelectedIndex = Usuario.IdRol - 1;  // Restamos 1 si los roles empiezan desde 1
        }

        // Guardar los cambios del usuario
        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            // Validar campos
            if (string.IsNullOrEmpty(EntryPassword.Text))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos.", "OK");
                return;
            }

            // Actualizar la información del usuario
            Usuario.Contraseña = EntryPassword.Text;

            // Asignar el IdRol correctamente, tomando el índice seleccionado
            switch (PickerRole.SelectedIndex)
            {
                case 0:
                    Usuario.IdRol = 1;  // "Administrador"
                    break;
                case 1:
                    Usuario.IdRol = 2;  // "Encuestador"
                    break;
                case 2:
                    Usuario.IdRol = 3;  // "Desarrollador"
                    break;
                default:
                    Usuario.IdRol = 0;  // Valor por defecto o inválido
                    break;
            }

            // Llamar al servicio para actualizar el usuario en la base de datos
            var userService = new UserService();  // Suponiendo que tienes un servicio similar al de agregar
            var result = await userService.UpdateUserAsync(Usuario);

            if (result)
            {
                await DisplayAlert("Éxito", "Los cambios fueron guardados correctamente.", "OK");

                // Notificar que el usuario ha sido actualizado
                OnUserUpdated?.Invoke();

                await Navigation.PopAsync();  // Regresar a la vista anterior
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar los cambios. Intenta nuevamente.", "OK");
            }
        }

        // Cancelar la edición
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();  // Volver a la vista anterior sin guardar cambios
        }
    }
}
