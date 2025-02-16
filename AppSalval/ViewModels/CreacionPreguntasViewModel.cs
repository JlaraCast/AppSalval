using System.Collections.ObjectModel;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;
using Microsoft.Maui.Controls;

namespace AppSalval.ViewModels
{

    public class CreacionPreguntasViewModel : BindableObject
    {
        private readonly ApiServiceOpcionRespuesta _apiService;
        public Command AgregarOpcionRespuestaCommand { get; }
        public Command EliminarOpcionRespuestaCommand { get; }
        public Command CargarOpcionesRespuestaCommand { get; }
        private List<OpcionRespuestaDto> _opcionesRespuesta;
        private int _preguntaId;

        public List<OpcionRespuestaDto> OpcionesRespuesta
        {
            get => _opcionesRespuesta;
            set
            {
                _opcionesRespuesta = value;
                OnPropertyChanged();
            }
        }

      

        private readonly ApiServicePregunta _apiServicePregunta;

        public CreacionPreguntasViewModel()
        {
            _apiService = new ApiServiceOpcionRespuesta();
            _apiServicePregunta = new ApiServicePregunta();
            AgregarOpcionRespuestaCommand = new Command(async () => await AgregarOpcionRespuesta(_preguntaId));
            EliminarOpcionRespuestaCommand = new Command<OpcionRespuestaDto>(async (opcionRespuesta) => await EliminarOpcionRespuesta(opcionRespuesta));
            CargarOpcionesRespuestaCommand = new Command(async () => await CargarOpcionesRespuesta());
            CrearPregunta();
        }

        private async Task CrearPregunta()
        {
            try
            {
                var nuevaPregunta = new PreguntaDto {  TextoPregunta = "Nueva Pregunta", TipoPregunta = "SelecciónMultiple" };
                var resultado = await _apiServicePregunta.AddPregunta(nuevaPregunta);
                if (resultado)
                {
                    var preguntasExistentes = await _apiServicePregunta.GetPreguntas();
                    _preguntaId = preguntasExistentes.Any() ? preguntasExistentes.Max(p => p.IdPregunta) : 0;
                    await Application.Current.MainPage.DisplayAlert("Éxito", $"La pregunta fue creada exitosamente.{_preguntaId}", "OK");
                    await CargarOpcionesRespuesta();
                    await Application.Current.MainPage.DisplayAlert("Éxito", "La pregunta fue creada exitosamente.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear la pregunta. Intenta nuevamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al crear la pregunta: {ex.Message}", "OK");
            }
        }

        private async Task CargarOpcionesRespuesta()
        {
            try
            {
                List<OpcionRespuestaDto> opciones = await _apiService.GetOpcionRespuestaById(_preguntaId);

                if (opciones != null && opciones.Count > 0)
                {
                    OpcionesRespuesta = opciones;
                }
                else
                {
                    OpcionesRespuesta= new List<OpcionRespuestaDto>();
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay opciones de respuesta disponibles", "OK");
                    
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las opciones de respuesta: {ex.Message}", "OK");
            }
        }

        private async Task AgregarOpcionRespuesta(int preguntaId)
        {
            await Application.Current.MainPage.DisplayAlert("Notificación", "Se ha agregado una nueva opción de respuesta.", "OK");

            var nuevaOpcion = new OpcionRespuestaDto { NombreOpcion = string.Empty, IdPregunta = preguntaId };
            var resultado = await _apiService.AddOpcionRespuesta(nuevaOpcion);
            if (resultado != null)
            {
                OpcionesRespuesta.Add(resultado);
                OnPropertyChanged(nameof(OpcionesRespuesta));
                await CargarOpcionesRespuesta(); // Refrescar la lista después de agregar
            }
        }

        private async Task EliminarOpcionRespuesta(OpcionRespuestaDto opcionRespuesta)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro de que desea borrar esta opción de respuesta?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    var resultado = await _apiService.DeleteOpcionRespuestaAsync(opcionRespuesta.IdOpcion);
                    if (resultado)
                    {
                        OpcionesRespuesta.Remove(opcionRespuesta);
                        OnPropertyChanged(nameof(OpcionesRespuesta));
                        await Application.Current.MainPage.DisplayAlert("Éxito", "La opción de respuesta fue eliminada correctamente.", "OK");
                        await CargarOpcionesRespuesta();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la opción de respuesta. Intenta nuevamente.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }
    }
}
