using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppSalval.DTOS_API;
using AppSalval.Services;
using AppSalval.Views;
using Microsoft.Maui.Controls;

namespace AppSalval.ViewModels
{
    public class GestionRespuestasViewModel : BindableObject
    {
        private readonly ApiServiceRespuestas _apiService;
        public Command BorrarCommand { get; }
        public Command EditarCommand { get; }
        private List<RespuestasDTO> _listaRespuestas;

        public List<RespuestasDTO> ListaRespuestas
        {
            get => _listaRespuestas;
            set
            {
                _listaRespuestas = value;
                OnPropertyChanged();
            }
        }

        public GestionRespuestasViewModel()
        {
            _apiService = new ApiServiceRespuestas();
            BorrarCommand = new Command<RespuestasDTO>(OnBorrarRespuestaClicked);
            EditarCommand = new Command<RespuestasDTO>(OnEditarRespuesta);
            CargarRespuestas();
        }

        private async Task CargarRespuestas()
        {
            try
            {
                List<RespuestasDTO> respuestas = await _apiService.GetRespuestas();

                if (respuestas != null && respuestas.Count > 0)
                {
                    foreach (var respuesta in respuestas)
                    {
                        if (string.IsNullOrEmpty(respuesta.IdentificacionEncuestado))
                        {

                            respuesta.IdentificacionEncuestado = "Anónimo";
                        }
                    }
                    ListaRespuestas = respuestas;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay respuestas disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las respuestas: {ex.Message}", "OK");
            }
        }

        private async void OnBorrarRespuestaClicked(RespuestasDTO respuesta)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro de que desea borrar esta respuesta?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    // Llamar al servicio para eliminar la respuesta
                    var result = await _apiService.DeleteRespuestaAsync(respuesta.IdRespuesta);
                    if (result)
                    {
                        ListaRespuestas.Remove(respuesta);  // Actualizamos la lista
                        OnPropertyChanged(nameof(ListaRespuestas));  // Notificar el cambio
                        await Application.Current.MainPage.DisplayAlert("Éxito", "La respuesta fue eliminada correctamente.", "OK");

                        CargarRespuestas();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la respuesta. Intenta nuevamente.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }

        private async void OnEditarRespuesta(RespuestasDTO respuesta)
        {
            var editarRespuestaPage = new EdicionRespuestas(respuesta);
            editarRespuestaPage.OnRespuestaUpdated += async () => await CargarRespuestas();
            await Application.Current.MainPage.Navigation.PushAsync(editarRespuestaPage);
        }
    }
}