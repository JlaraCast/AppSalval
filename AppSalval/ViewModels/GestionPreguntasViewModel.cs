using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSalval.ViewModels;
using AppSalval.Models_Api;
using AppSalval.Services;
using AppSalval.DTOS_API;
namespace AppSalval.ViewModels
{
    public class GestionPreguntasViewModel : BaseViewModel
    {
        
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcionRespuesta;
        private readonly INavigation _navigation;

        public GestionPreguntasViewModel(INavigation navigation)
        {
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceOpcionRespuesta = new ApiServiceOpcionRespuesta();
            _navigation = navigation;
            //PreguntasDtos = new ObservableCollection<PreguntaViewModel>();
            // Inicialización de propiedades o comandos
        }
        /*
        public async Task ObservableCollection<PreguntaViewModel> PreguntasDtos { get; set; }
        
        private async Task CargarPreguntas()
        {
            var preguntas = await _apiServicePregunta.GetPreguntas();

            if (preguntas == null || preguntas.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Información", "No hay preguntas disponibles", "OK");
                return;
            }

            PreguntasDtos.Clear(); // Limpiar antes de cargar nuevas preguntas

            foreach (var pregunta in preguntas)
            {
                // Obtener las opciones de respuesta para cada pregunta
                var opciones = await _apiServiceOpcionRespuesta.GetOpcionRespuestaById(pregunta.IdPregunta) ?? new List<OpcionRespuestaDto>();

                // Convertir opciones a ViewModel
                var opcionesViewModel = new ObservableCollection<OpcionRespuestaViewModel>();

                foreach (var o in opciones)
                {
                    opcionesViewModel.Add(new OpcionRespuestaViewModel
                    {
                        OpcionId = o.IdOpcion,
                        NombreOpcion = o.NombreOpcion,
                        IdPregunta = o.IdPregunta,
                        IsSelected = false
                    });
                }

                // Agregar la pregunta con sus opciones a la lista
                PreguntasDtos.Add(new PreguntaViewModel
                {
                    PreguntaId = pregunta.IdPregunta,
                    TextoPregunta = pregunta.TextoPregunta,
                    Opciones = opcionesViewModel
                });
            }
        }

        */

    }
}
