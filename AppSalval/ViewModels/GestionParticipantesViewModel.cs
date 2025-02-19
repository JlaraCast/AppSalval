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
    public class GestionParticipantesViewModel : BindableObject
    {
        private readonly ApiServiceRespuestas _apiServiceRespuestas;
        private readonly ApiServiceEncuestado _apiServiceEncuestado;
        public Command VerFormularioCommand { get; }
        public Command EditarCommand { get; }
        private List<EncuestadoExtendidoDTO> _listaRespuestas;

        public List<EncuestadoExtendidoDTO> ListaRespuestas
        {
            get => _listaRespuestas;
            set
            {
                _listaRespuestas = value;
                OnPropertyChanged();
            }
        }

        public GestionParticipantesViewModel()
        {
            _apiServiceRespuestas = new ApiServiceRespuestas();
            _apiServiceEncuestado = new ApiServiceEncuestado();
            VerFormularioCommand = new Command<EncuestadoExtendidoDTO>(OnVerFormularioClicked);
            EditarCommand = new Command<EncuestadoExtendidoDTO>(OnEditarRespuesta);
            CargarRespuestas();
        }

        private async Task CargarRespuestas()
        {
            try
            {
                List<RespuestasDTO> respuestas = await _apiServiceRespuestas.GetRespuestas();
                var listaRespuestasExtendidas = new List<EncuestadoExtendidoDTO>();

                if (respuestas != null && respuestas.Count > 0)
                {
                    foreach (var respuesta in respuestas)
                    {
                        EncuestadoDto encuestado = null;

                        if (!string.IsNullOrEmpty(respuesta.IdentificacionEncuestado))
                        {
                            encuestado = await _apiServiceEncuestado.GetEncuestado(respuesta.IdentificacionEncuestado);
                        }

                        var respuestaExtendida = new EncuestadoExtendidoDTO(encuestado ?? new EncuestadoDto
                        {
                            TipoIdentificacion = "Anónimo",
                            NombreCompleto = "Anónimo",
                            FechaNacimiento = DateTime.MinValue,
                            Sexo = "Anónimo"
                        })
                        {
                            IdRespuesta = respuesta.IdRespuesta,
                            FechaRespuesta = respuesta.FechaRespuesta,
                            IdFormulario = respuesta.IdFormulario
                        };

                        listaRespuestasExtendidas.Add(respuestaExtendida);
                    }

                    ListaRespuestas = listaRespuestasExtendidas;
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

        private void OnVerFormularioClicked(EncuestadoExtendidoDTO respuesta)
        {
            // Método vacío para la acción del botón Ver Formulario
        }

        private void OnEditarRespuesta(EncuestadoExtendidoDTO respuesta)
        {
            // Método vacío para la acción del botón Editar
        }
    }
}