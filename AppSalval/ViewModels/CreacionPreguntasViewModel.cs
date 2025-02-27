﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;
using Microsoft.Maui.Controls;

namespace AppSalval.ViewModels
{
    // Clase ViewModel para la creación de preguntas
    public class CreacionPreguntasViewModel : BindableObject
    {
        // Servicios utilizados por el ViewModel
        private readonly ApiServiceOpcionRespuesta _apiService;
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly RecomService _apiServiceRecomendaciones;
        private readonly FactorService _apiServiceFactoresRiesgo;
        private readonly ApiServiceReglaOpcion _apiServiceReglaOpcion;

        // Variables privadas
        private int _preguntaId = -1;
        private ObservableCollection<OpcionRespuestaDtoExtendida> _opcionesRespuesta;
        private List<Recomendacion> _recomendaciones;
        private List<FactorRiesgo> _factoresRiesgo;
        private string _textoPregunta;
        private string _tipoPregunta;
        private int _tipoPreguntaIndex;

        // Constructor por defecto
        public CreacionPreguntasViewModel()
        {
            // Inicialización de servicios
            _apiService = new ApiServiceOpcionRespuesta();
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceRecomendaciones = new RecomService();
            _apiServiceFactoresRiesgo = new FactorService();
            _apiServiceReglaOpcion = new ApiServiceReglaOpcion();

            // Inicialización de comandos
            AgregarOpcionRespuestaCommand = new Command(async () => await ControlCrearOpcionesRespuesta());
            EliminarOpcionRespuestaCommand = new Command<OpcionRespuestaDtoExtendida>(async (opcionRespuesta) => await EliminarOpcionRespuesta(opcionRespuesta));
            CargarOpcionesRespuestaCommand = new Command(async () => await CargarOpcionesRespuesta());
            GuardarCambiosCommand = new Command(async () => await GuardarCambios());

            // Inicialización de colecciones
            OpcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>();
            RecomendacionesComboBox = new ObservableCollection<string>();
            FactoresRiesgoComboBox = new ObservableCollection<string>();

            // Carga de datos iniciales
            CargarDatosIniciales();
        }

        // Constructor con parámetro de ID de pregunta
        public CreacionPreguntasViewModel(int idPregunta) : this()
        {
            _ = CargarPregunta(idPregunta);
        }

        // Método llamado cuando la página reaparece
        public void OnPageReappearing()
        {
            _preguntaId = -1;
        }

        // Propiedad para las opciones de respuesta
        public ObservableCollection<OpcionRespuestaDtoExtendida> OpcionesRespuesta
        {
            get => _opcionesRespuesta;
            set
            {
                _opcionesRespuesta = value;
                OnPropertyChanged();
            }
        }

        // Propiedad para las recomendaciones
        public List<Recomendacion> Recomendaciones
        {
            get => _recomendaciones;
            set
            {
                _recomendaciones = value;
                OnPropertyChanged();
                RecomendacionesComboBox = new ObservableCollection<string>(_recomendaciones.Select(r => $"{r.IdRecomendacion} - {r.TextoRecomendacion}"));
                OnPropertyChanged(nameof(RecomendacionesComboBox));
            }
        }

        // Propiedad para los factores de riesgo
        public List<FactorRiesgo> FactoresRiesgo
        {
            get => _factoresRiesgo;
            set
            {
                _factoresRiesgo = value;
                OnPropertyChanged();
                FactoresRiesgoComboBox = new ObservableCollection<string>(_factoresRiesgo.Select(f => $"{f.IdFactor} - {f.TextoFactor}"));
                OnPropertyChanged(nameof(FactoresRiesgoComboBox));
            }
        }

        // Propiedad para el ComboBox de recomendaciones
        public ObservableCollection<string> RecomendacionesComboBox { get; private set; }

        // Propiedad para el ComboBox de factores de riesgo
        public ObservableCollection<string> FactoresRiesgoComboBox { get; private set; }

        // Propiedad para el texto de la pregunta
        public string TextoPregunta
        {
            get => _textoPregunta;
            set
            {
                _textoPregunta = value;
                OnPropertyChanged();
            }
        }

        // Propiedad para el tipo de pregunta
        public string TipoPregunta
        {
            get => _tipoPregunta;
            set
            {
                _tipoPregunta = value;
                OnPropertyChanged();
            }
        }

        // Propiedad para el índice del tipo de pregunta
        public int TipoPreguntaIndex
        {
            get => _tipoPreguntaIndex;
            set
            {
                _tipoPreguntaIndex = value;
                OnPropertyChanged();
            }
        }

        // Comandos
        public ICommand AgregarOpcionRespuestaCommand { get; }
        public ICommand EliminarOpcionRespuestaCommand { get; }
        public ICommand CargarOpcionesRespuestaCommand { get; }
        public ICommand GuardarCambiosCommand { get; }

        // Método para cargar datos iniciales
        private async void CargarDatosIniciales()
        {
            await CargarRecomendaciones();
            await CargarFactoresRiesgo();
        }

        // Método para cargar recomendaciones
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

        // Método para cargar factores de riesgo
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

        // Método para crear una nueva pregunta
        private async Task CrearPregunta()
        {
            try
            {
                var nuevaPregunta = new PreguntaDto { TextoPregunta = "texttemp", TipoPregunta = "tipotemp" };
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

        // Método para controlar la creación de opciones de respuesta
        private async Task ControlCrearOpcionesRespuesta()
        {
            if (_preguntaId == -1)
            {
                await CrearPregunta();
            }

            await AgregarOpcionRespuesta();
        }

        // Método para cargar opciones de respuesta
        private async Task CargarOpcionesRespuesta()
        {
            try
            {
                var opciones = await _apiService.GetOpcionRespuestaById(_preguntaId);
                var opcionesExtendidas = new List<OpcionRespuestaDtoExtendida>();

                foreach (var opcion in opciones)
                {
                    var reglasOpcion = await _apiServiceReglaOpcion.GetReglaOpcionByOpcionId(opcion.IdOpcion);
                    var reglaOpcion = reglasOpcion?.FirstOrDefault();

                    var opcionExtendida = new OpcionRespuestaDtoExtendida(opcion)
                    {
                        SeleccionadaRecomendacion = reglaOpcion?.IdRecomendacion.ToString(),
                        SeleccionadaRiesgo = reglaOpcion?.IdFactorRiesgo.ToString(),
                        Condicion = double.TryParse(reglaOpcion?.Condicion, out double condicion) ? condicion : 0
                    };

                    // Asigna los índices seleccionados
                    opcionExtendida.SeleccionadaRecomendacionIndex = RecomendacionesComboBox.IndexOf($"{opcionExtendida.SeleccionadaRecomendacion} - {Recomendaciones?.FirstOrDefault(r => r.IdRecomendacion.ToString() == opcionExtendida.SeleccionadaRecomendacion)?.TextoRecomendacion}");
                    opcionExtendida.SeleccionadaRiesgoIndex = FactoresRiesgoComboBox.IndexOf($"{opcionExtendida.SeleccionadaRiesgo} - {FactoresRiesgo?.FirstOrDefault(f => f.IdFactor.ToString() == opcionExtendida.SeleccionadaRiesgo)?.TextoFactor}");

                    opcionesExtendidas.Add(opcionExtendida);
                }

                OpcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>(opcionesExtendidas);

                OnPropertyChanged(nameof(OpcionesRespuesta));

                if (!OpcionesRespuesta.Any())
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay opciones de respuesta disponibles", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Opciones de respuesta cargadas correctamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar las opciones de respuesta: {ex.Message}", "OK");
            }
        }

        // Método para agregar una nueva opción de respuesta
        private async Task AgregarOpcionRespuesta()
        {
            var nuevaOpcion = new OpcionRespuestaDto { NombreOpcion = string.Empty, IdPregunta = _preguntaId };
            var resultado = await _apiService.AddOpcionRespuesta(nuevaOpcion);
            if (resultado != null)
            {
                OpcionesRespuesta.Add(new OpcionRespuestaDtoExtendida(resultado));
                OnPropertyChanged(nameof(OpcionesRespuesta));
                await CargarOpcionesRespuesta();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Opción de respuesta agregada correctamente.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo agregar la opción de respuesta. Intenta nuevamente.", "OK");
            }
        }

        // Método para eliminar una opción de respuesta
        private async Task EliminarOpcionRespuesta(OpcionRespuestaDtoExtendida opcionRespuesta)
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

        // Método para cargar una pregunta por su ID
        public async Task CargarPregunta(int preguntaId)
        {
            try
            {
                await CargarRecomendaciones();
                await CargarFactoresRiesgo();

                var pregunta = await _apiServicePregunta.GetPreguntaById(preguntaId);
                if (pregunta == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Pregunta no encontrada.", "OK");
                    return;
                }

                _preguntaId = pregunta.IdPregunta;
                TextoPregunta = pregunta.TextoPregunta;
                TipoPregunta = pregunta.TipoPregunta;
                TipoPreguntaIndex = TipoPregunta == "Selección Múltiple" ? 0 : 1;

                var opciones = await _apiService.GetOpcionRespuestaById(_preguntaId);
                var opcionesExtendidas = new List<OpcionRespuestaDtoExtendida>();

                foreach (var opcion in opciones)
                {
                    var reglasOpcion = await _apiServiceReglaOpcion.GetReglaOpcionByOpcionId(opcion.IdOpcion);
                    var reglaOpcion = reglasOpcion?.FirstOrDefault();

                    var opcionExtendida = new OpcionRespuestaDtoExtendida(opcion)
                    {
                        SeleccionadaRecomendacion = reglaOpcion?.IdRecomendacion.ToString(),
                        SeleccionadaRiesgo = reglaOpcion?.IdFactorRiesgo.ToString(),
                        Condicion = double.TryParse(reglaOpcion?.Condicion, out double condicion) ? condicion : 0
                    };

                    // Asigna los índices seleccionados
                    opcionExtendida.SeleccionadaRecomendacionIndex = RecomendacionesComboBox.IndexOf($"{opcionExtendida.SeleccionadaRecomendacion} - {Recomendaciones?.FirstOrDefault(r => r.IdRecomendacion.ToString() == opcionExtendida.SeleccionadaRecomendacion)?.TextoRecomendacion}");
                    opcionExtendida.SeleccionadaRiesgoIndex = FactoresRiesgoComboBox.IndexOf($"{opcionExtendida.SeleccionadaRiesgo} - {FactoresRiesgo?.FirstOrDefault(f => f.IdFactor.ToString() == opcionExtendida.SeleccionadaRiesgo)?.TextoFactor}");

                    opcionesExtendidas.Add(opcionExtendida);
                }

                OpcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>(opcionesExtendidas);

                OnPropertyChanged(nameof(OpcionesRespuesta));
                OnPropertyChanged(nameof(TextoPregunta));
                OnPropertyChanged(nameof(TipoPregunta));
                OnPropertyChanged(nameof(TipoPreguntaIndex));
                OnPropertyChanged(nameof(RecomendacionesComboBox));
                OnPropertyChanged(nameof(FactoresRiesgoComboBox));

                await Application.Current.MainPage.DisplayAlert("Éxito", "Pregunta cargada correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar la pregunta y sus opciones: {ex.Message}", "OK");
            }
        }

        // Método para guardar cambios
        private async Task GuardarCambios()
        {
            try
            {
                if (OpcionesRespuesta.Any(opcion => string.IsNullOrEmpty(opcion.NombreOpcion) || opcion.SeleccionadaRecomendacionIndex == -1 || opcion.SeleccionadaRiesgoIndex == -1 || opcion.Condicion < 0 || opcion.Condicion > 10))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debe completar todos los campos para cada opción de respuesta y asegurarse de que la condición esté entre 0 y 10.", "OK");
                    return;
                }

                var pregunta = new PreguntaDto
                {
                    IdPregunta = _preguntaId,
                    TextoPregunta = TextoPregunta,
                    TipoPregunta = TipoPregunta
                };
                await _apiServicePregunta.EditPregunta(pregunta);

                foreach (var opcion in OpcionesRespuesta)
                {
                    await _apiService.EditOpcionRespuesta(opcion);

                    var reglaOpcion = new ReglaOpcionDto
                    {
                        IdOpcion = opcion.IdOpcion,
                        IdRecomendacion = int.Parse(RecomendacionesComboBox[opcion.SeleccionadaRecomendacionIndex].Split(' ')[0]),
                        IdFactorRiesgo = int.Parse(FactoresRiesgoComboBox[opcion.SeleccionadaRiesgoIndex].Split(' ')[0]),
                        Condicion = opcion.Condicion.ToString()
                    };
                    await _apiServiceReglaOpcion.AddReglaOpcion(reglaOpcion);
                }

                await Application.Current.MainPage.DisplayAlert("Éxito", "Los cambios fueron guardados exitosamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al guardar los cambios: {ex.Message}", "OK");
            }
        }
    }

    // Clase extendida para las opciones de respuesta
    public class OpcionRespuestaDtoExtendida : OpcionRespuestaDto
    {
        public OpcionRespuestaDtoExtendida(OpcionRespuestaDto opcionRespuestaDto)
        {
            IdOpcion = opcionRespuestaDto.IdOpcion;
            NombreOpcion = opcionRespuestaDto.NombreOpcion;
            IdPregunta = opcionRespuestaDto.IdPregunta;
        }

        public string SeleccionadaRecomendacion { get; set; }
        public string SeleccionadaRiesgo { get; set; }
        public double Condicion { get; set; }
        public int SeleccionadaRecomendacionIndex { get; set; }
        public int SeleccionadaRiesgoIndex { get; set; }
    }
}