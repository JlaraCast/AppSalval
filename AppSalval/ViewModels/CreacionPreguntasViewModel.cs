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
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly RecomService _apiServiceRecomendaciones;
        private readonly FactorService _apiServiceFactoresRiesgo;
        private int _preguntaId = -1;
        private List<OpcionRespuestaDto> _opcionesRespuesta;
        private List<Recomendacion> _recomendaciones;
        private List<FactorRiesgo> _factoresRiesgo;
        private string _seleccionadaRecomendacion;
        private string _seleccionadaRiesgo;
        private ObservableCollection<string> _recomendacionesComboBox;
        private ObservableCollection<string> _factoresRiesgoComboBox;

        public CreacionPreguntasViewModel()
        {
            _apiService = new ApiServiceOpcionRespuesta();
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceRecomendaciones = new RecomService();
            _apiServiceFactoresRiesgo = new FactorService();
            AgregarOpcionRespuestaCommand = new Command(async () => await ControlCrearOpcionesRespuesta());
            EliminarOpcionRespuestaCommand = new Command<OpcionRespuestaDto>(async (opcionRespuesta) => await EliminarOpcionRespuesta(opcionRespuesta));
            CargarOpcionesRespuestaCommand = new Command(async () => await CargarOpcionesRespuesta());
            CargarRecomendacionesCommand = new Command(async () => await CargarRecomendaciones());
            CargarFactoresRiesgoCommand = new Command(async () => await CargarFactoresRiesgo());
            CargarRecomendaciones();
            CargarFactoresRiesgo();
        }

        public List<OpcionRespuestaDto> OpcionesRespuesta
        {
            get => _opcionesRespuesta;
            set
            {
                _opcionesRespuesta = value;
                OnPropertyChanged();
            }
        }

        public List<Recomendacion> Recomendaciones
        {
            get => _recomendaciones;
            set
            {
                _recomendaciones = value;
                OnPropertyChanged();
                RecomendacionesComboBox = new ObservableCollection<string>(_recomendaciones.Select(r => $"{r.IdRecomendacion} - {r.TextoRecomendacion}"));
            }
        }

        public List<FactorRiesgo> FactoresRiesgo
        {
            get => _factoresRiesgo;
            set
            {
                _factoresRiesgo = value;
                OnPropertyChanged();
                FactoresRiesgoComboBox = new ObservableCollection<string>(_factoresRiesgo.Select(f => $"{f.IdFactor} - {f.TextoFactor}"));
            }
        }

        public ObservableCollection<string> RecomendacionesComboBox
        {
            get => _recomendacionesComboBox;
            private set
            {
                _recomendacionesComboBox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> FactoresRiesgoComboBox
        {
            get => _factoresRiesgoComboBox;
            private set
            {
                _factoresRiesgoComboBox = value;
                OnPropertyChanged();
            }
        }

        public string SeleccionadaRecomendacion
        {
            get => _seleccionadaRecomendacion;
            set
            {
                _seleccionadaRecomendacion = value;
                OnPropertyChanged();
            }
        }

        public string SeleccionadaRiesgo
        {
            get => _seleccionadaRiesgo;
            set
            {
                _seleccionadaRiesgo = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarOpcionRespuestaCommand { get; }
        public ICommand EliminarOpcionRespuestaCommand { get; }
        public ICommand CargarOpcionesRespuestaCommand { get; }
        public ICommand CargarRecomendacionesCommand { get; }
        public ICommand CargarFactoresRiesgoCommand { get; }

        private async Task CargarRecomendaciones()
        {
            try
            {
                Recomendaciones = await _apiServiceRecomendaciones.GetRecomendacionesAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las recomendaciones: {ex.Message}", "OK");
            }
        }

        private async Task CargarFactoresRiesgo()
        {
            try
            {
                FactoresRiesgo = await _apiServiceFactoresRiesgo.GetFactoresAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar los factores de riesgo: {ex.Message}", "OK");
            }
        }

        private async Task CrearPregunta()
        {
            try
            {
                var nuevaPregunta = new PreguntaDto { TextoPregunta = "Nueva Pregunta", TipoPregunta = "SelecciónMultiple" };
                var resultado = await _apiServicePregunta.AddPregunta(nuevaPregunta);
                if (resultado)
                {
                    var preguntasExistentes = await _apiServicePregunta.GetPreguntas();
                    _preguntaId = preguntasExistentes.Any() ? preguntasExistentes.Max(p => p.IdPregunta) : 0;
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

        private async Task ControlCrearOpcionesRespuesta()
        {
            if (_preguntaId == -1)
            {
                await CrearPregunta();
                await AgregarOpcionRespuesta();
            }
            else
            {
                await AgregarOpcionRespuesta();
            }
        }

        private async Task CargarOpcionesRespuesta()
        {
            try
            {
                List<OpcionRespuestaDto> opciones = await _apiService.GetOpcionRespuestaById(_preguntaId);
                OpcionesRespuesta = opciones ?? new List<OpcionRespuestaDto>();
                if (OpcionesRespuesta.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay opciones de respuesta disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las opciones de respuesta: {ex.Message}", "OK");
            }
        }

        private async Task AgregarOpcionRespuesta()
        {
            await Application.Current.MainPage.DisplayAlert("Notificación", "Se ha agregado una nueva opción de respuesta.", "OK");

            var nuevaOpcion = new OpcionRespuestaDto { NombreOpcion = string.Empty, IdPregunta = _preguntaId };
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