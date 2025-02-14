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
                            IdentificacionEncuestado = string.IsNullOrEmpty(r.IdentificacionEncuestado) ? "Anónimo" : r.IdentificacionEncuestado,
                            EditarCommand = EditarCommand,
                            BorrarCommand = BorrarCommand
                        })
                        .ToList();

                    ListaRespuestas.ItemsSource = respuestaConFormato;
                }
                else
                {
                    await DisplayAlert("Información", "No hay respuestas disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar las respuestas: {ex.Message}", "OK");
            }
        }

        private async void OnBorrarRespuestaClicked(RespuestasDTO respuesta)
        {
            // Lógica para borrar la respuesta
            // Aquí puedes llamar a un método del ApiService para borrar la respuesta en la API
            // y luego eliminarla de la colección Respuestas
            var confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea borrar esta respuesta?", "Sí", "No");
            if (confirm)
            {
              //  await _apiService.DeleteRespuesta(respuesta.IdRespuesta);
                CargarRespuestas();
            }
        }

        private void OnEditarRespuesta(RespuestasDTO respuesta)
        {
            // Lógica para editar la respuesta
            // Aquí puedes navegar a una página de edición o mostrar un formulario de edición
        }

        private void OnAnadirClicked(object sender, EventArgs e)
        {
            // Lógica para añadir una nueva respuesta
            // Aquí puedes navegar a una página de creación o mostrar un formulario de creación
        }

    }
}