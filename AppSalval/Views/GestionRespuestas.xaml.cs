using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppSalval.DTOS_API;
using AppSalval.Services;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class GestionRespuestas : ContentPage
    {
        private readonly ApiServiceRespuestas _apiService;
        public Command BorrarCommand { get; }
        public Command EditarCommand { get; }

        public GestionRespuestas()
        {
            InitializeComponent();
            _apiService = new ApiServiceRespuestas();
            BorrarCommand = new Command<RespuestasDTO>(OnBorrarRespuestaClicked);
            EditarCommand = new Command<RespuestasDTO>(OnEditarRespuesta);
            CargarRespuestas();
        }

        private async void CargarRespuestas()
        {
            try
            {
                List<RespuestasDTO> respuestas = await _apiService.GetRespuestas();

                if (respuestas != null && respuestas.Count > 0)
                {
                    var respuestaConFormato = respuestas
                        .Select(r => new
                        {
                            r.IdFormulario,
                            r.IdRespuesta,
                            r.FechaRespuesta,
                            IdentificacionEncuestado = string.IsNullOrEmpty(r.IdentificacionEncuestado) ? "An�nimo" : r.IdentificacionEncuestado,
                            EditarCommand = EditarCommand,
                            BorrarCommand = BorrarCommand
                        })
                        .ToList();

                    ListaRespuestas.ItemsSource = respuestaConFormato;
                }
                else
                {
                    await DisplayAlert("Informaci�n", "No hay respuestas disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurri� un error al cargar las respuestas: {ex.Message}", "OK");
            }
        }

        private async void OnBorrarRespuestaClicked(RespuestasDTO respuesta)
        {
            // L�gica para borrar la respuesta
            // Aqu� puedes llamar a un m�todo del ApiService para borrar la respuesta en la API
            // y luego eliminarla de la colecci�n Respuestas
            var confirm = await DisplayAlert("Confirmaci�n", "�Est� seguro de que desea borrar esta respuesta?", "S�", "No");
            if (confirm)
            {
              //  await _apiService.DeleteRespuesta(respuesta.IdRespuesta);
                CargarRespuestas();
            }
        }

        private void OnEditarRespuesta(RespuestasDTO respuesta)
        {
            // L�gica para editar la respuesta
            // Aqu� puedes navegar a una p�gina de edici�n o mostrar un formulario de edici�n
        }

        private void OnAnadirClicked(object sender, EventArgs e)
        {
            // L�gica para a�adir una nueva respuesta
            // Aqu� puedes navegar a una p�gina de creaci�n o mostrar un formulario de creaci�n
        }

    }
}