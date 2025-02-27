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
        // Servicios para obtener datos de respuestas y encuestados
        private readonly ApiServiceRespuestas _apiServiceRespuestas;
        private readonly ApiServiceEncuestado _apiServiceEncuestado;

        // Comandos para las acciones de los botones
        public Command VerFormularioCommand { get; }
        public Command EditarCommand { get; }

        // Lista de respuestas extendidas
        private List<EncuestadoExtendidoDTO> _listaRespuestas;

        // Propiedad para la lista de respuestas extendidas
        public List<EncuestadoExtendidoDTO> ListaRespuestas
        {
            get => _listaRespuestas;
            set
            {
                _listaRespuestas = value;
                OnPropertyChanged();
            }
        }

        // Constructor
        public GestionParticipantesViewModel()
        {
            // Inicialización de servicios
            _apiServiceRespuestas = new ApiServiceRespuestas();
            _apiServiceEncuestado = new ApiServiceEncuestado();

            // Inicialización de comandos
            VerFormularioCommand = new Command<EncuestadoExtendidoDTO>(OnVerFormularioClicked);
            EditarCommand = new Command<EncuestadoExtendidoDTO>(OnEditarRespuesta);

            // Cargar respuestas al inicializar
            CargarRespuestas();
        }

        // Método para cargar respuestas desde la API
        private async Task CargarRespuestas()
        {
            try
            {
                // Obtener respuestas desde el servicio
                List<RespuestasDTO> respuestas = await _apiServiceRespuestas.GetRespuestas();
                var listaRespuestasExtendidas = new List<EncuestadoExtendidoDTO>();

                // Verificar si hay respuestas
                if (respuestas != null && respuestas.Count > 0)
                {
                    // Procesar cada respuesta
                    foreach (var respuesta in respuestas)
                    {
                        EncuestadoDto encuestado = null;
                        EncuestadoExtendidoDTO respuestaExtendida;

                        // Verificar si la respuesta tiene identificación de encuestado
                        if (!string.IsNullOrEmpty(respuesta.IdentificacionEncuestado))
                        {
                            // Obtener encuestado desde el servicio
                            encuestado = await _apiServiceEncuestado.GetEncuestado(respuesta.IdentificacionEncuestado);
                            respuestaExtendida = new EncuestadoExtendidoDTO(encuestado)
                            {
                                IdRespuesta = respuesta.IdRespuesta,
                                FechaRespuesta = respuesta.FechaRespuesta,
                                IdFormulario = respuesta.IdFormulario
                            };
                        }
                        else
                        {
                            // Crear respuesta extendida con datos anónimos
                            respuestaExtendida = new EncuestadoExtendidoDTO(encuestado ?? new EncuestadoDto
                            {
                                Identificacion = "Anónimo",
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
                        }

                        // Agregar respuesta extendida a la lista
                        listaRespuestasExtendidas.Add(respuestaExtendida);
                    }

                    // Asignar lista de respuestas extendidas a la propiedad
                    ListaRespuestas = listaRespuestasExtendidas;
                }
                else
                {
                    // Mostrar alerta si no hay respuestas disponibles
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay respuestas disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                // Mostrar alerta en caso de error
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las respuestas: {ex.Message}", "OK");
            }
        }

        // Método para la acción del botón Ver Formulario
        private void OnVerFormularioClicked(EncuestadoExtendidoDTO respuesta)
        {
            // Método vacío para la acción del botón Ver Formulario
        }

        // Método para la acción del botón Editar
        private void OnEditarRespuesta(EncuestadoExtendidoDTO respuesta)
        {
            // Método vacío para la acción del botón Editar
        }
    }
}