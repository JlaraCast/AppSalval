// Modificaci�n en InfoPersonal.xaml.cs
using Microsoft.Maui.Controls;
using AppSalval.Services;
using AppSalval.DTOS_API;
using System;
using System.Threading.Tasks;

namespace AppSalval.Views
{
    public partial class InfoPersonal : ContentPage
    {
        // Identificador del formulario
        private int _idFormulario;
        // T�tulo del formulario
        private string _tituloFormulario;
        // Servicio API para manejar encuestados
        private readonly ApiServiceEncuestado _apiServiceEncuestado;

        // Constructor que inicializa el formulario y el servicio API
        public InfoPersonal(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _idFormulario = idFormulario;
            _tituloFormulario = tituloFormulario;
            _apiServiceEncuestado = new ApiServiceEncuestado();
        }

        // M�todo que se ejecuta al hacer clic en el bot�n "Continuar"
        private async void OnContinuarClicked(object sender, EventArgs e)
        {
            // Crear un nuevo objeto EncuestadoDto con los datos ingresados
            var encuestado = new EncuestadoDto
            {
                Identificacion = CedulaEntry.Text,
                TipoIdentificacion = "C�dula",
                NombreCompleto = NombreEntry.Text,
                FechaNacimiento = FechaNacimientoPicker.Date,
                Sexo = GeneroPicker.SelectedItem?.ToString(),
                Habilitado = true
            };

            // Llamar al servicio API para agregar el encuestado
            bool resultado = await _apiServiceEncuestado.AddEncuestado(encuestado);

            // Mostrar un mensaje de �xito o error seg�n el resultado
            if (resultado)
            {
                await DisplayAlert("�xito", "Encuestado registrado correctamente.", "OK");
                await Navigation.PushAsync(new AplicarFormulario(_idFormulario, _tituloFormulario));
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar al encuestado.", "OK");
            }
        }

        // M�todo que se ejecuta al hacer clic en el bot�n "Cancelar"
        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            // Volver a la p�gina anterior
            await Navigation.PopAsync();
        }
    }
}