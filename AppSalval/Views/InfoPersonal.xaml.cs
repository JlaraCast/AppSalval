// Modificación en InfoPersonal.xaml.cs
using Microsoft.Maui.Controls;
using AppSalval.Services;
using AppSalval.DTOS_API;
using System;
using System.Threading.Tasks;

namespace AppSalval.Views
{
    public partial class InfoPersonal : ContentPage
    {
        private int _idFormulario;
        private string _tituloFormulario;
        private readonly ApiServiceEncuestado _apiServiceEncuestado;

        public InfoPersonal(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _idFormulario = idFormulario;
            _tituloFormulario = tituloFormulario;
            _apiServiceEncuestado = new ApiServiceEncuestado();
        }

        private async void OnContinuarClicked(object sender, EventArgs e)
        {
            var encuestado = new EncuestadoDto
            {
                Identificacion = CedulaEntry.Text,
                TipoIdentificacion = "Cédula",
                NombreCompleto = NombreEntry.Text,
                FechaNacimiento = FechaNacimientoPicker.Date,
                Sexo = GeneroPicker.SelectedItem?.ToString(),
                Habilitado = true
            };

            bool resultado = await _apiServiceEncuestado.AddEncuestado(encuestado);

            if (resultado)
            {
                await DisplayAlert("Éxito", "Encuestado registrado correctamente.", "OK");
                await Navigation.PushAsync(new AplicarFormulario(_idFormulario, _tituloFormulario));
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar al encuestado.", "OK");
            }
        }


        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            // Volver a Formularios.xaml
            await Navigation.PopAsync();
        }
    }
}