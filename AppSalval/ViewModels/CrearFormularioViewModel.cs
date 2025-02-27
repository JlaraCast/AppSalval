using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Models_Api;
using AppSalval.Services;
using AppSalval.Views;

namespace AppSalval.ViewModels
{
    /// <summary>
    /// ViewModel para la creación de formularios.
    /// Maneja la lógica relacionada con la creación y gestión de formularios en la aplicación.
    /// </summary>
    public partial class CrearFormularioViewModel : BaseViewModel
    {
        private int idFormulario = -2;
        private readonly INavigation _navigation;
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly ApiServiceFormularios _apiServiceFormularios;
        private readonly ApiServiceFormularioPregunta _apiServiceFormularioPregunta;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcionRespuesta;
        private readonly ApiServiceReglaOpcion _apiServiceReglaOpcion;
        

        private string _titulo;
        private string _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _requiereDatosPersonales;
        private bool _habilitado;
        private ObservableCollection<OpcionRespuestaDtoExtendida> _opcionesRespuesta;

        /// <summary>
        /// Lista de preguntas asociadas al formulario.
        /// </summary>
        public ObservableCollection<PreguntaViewModel> _preguntasDtos { get; set; }

        /// <summary>
        /// Lista temporal de preguntas seleccionadas para el formulario.
        /// </summary>
        public ObservableCollection<PreguntaViewModel> PreguntasSeleccionadas { get; set; } = new ObservableCollection<PreguntaViewModel>();

        /// <summary>
        /// Comando para actualizar la lista de preguntas seleccionadas.
        /// </summary>
        public ICommand ActualizarPreguntasSeleccionadasCommand { get; }

        public ICommand BtnCancelar { get; }
        public ICommand BtnGuardar { get; }

        /// <summary>
        /// Lista de opciones de respuesta disponibles para las preguntas del formulario.
        /// </summary>
        public ObservableCollection<OpcionRespuestaDtoExtendida> OpcionesRespuesta
        {
            get => _opcionesRespuesta;
            set
            {
                _opcionesRespuesta = value;
                OnPropertyChanged();
            }
        }

        public ICommand CargarPreguntasCommand { get; }

        /// <summary>
        /// Constructor del ViewModel. Inicializa servicios, propiedades y comandos.
        /// </summary>
        /// <param name="navigation">Servicio de navegación.</param>
        public CrearFormularioViewModel(INavigation navigation)
        {
            // Inicialización de servicios de API
            _navigation = navigation;
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceFormularios = new ApiServiceFormularios();
            _apiServiceOpcionRespuesta = new ApiServiceOpcionRespuesta();
            _apiServiceReglaOpcion = new ApiServiceReglaOpcion();
            _apiServiceFormularioPregunta = new ApiServiceFormularioPregunta();

            // Inicialización de propiedades del formulario
            _titulo = string.Empty;
            _descripcion = string.Empty;
            _fechaInicio = DateTime.Now;
            _fechaFin = DateTime.Now;
            _requiereDatosPersonales = false;
            _habilitado = true;

            OpcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>();
            PreguntasDtos = new ObservableCollection<PreguntaViewModel>();

            // Inicialización de comandos
            ActualizarPreguntasSeleccionadasCommand = new Command<PreguntaViewModel>(ActualizarPreguntasSeleccionadas);
            BtnCancelar = new Command(async () => await _navigation.PushAsync(new GestionFormularios()));
            BtnGuardar = new Command(async () => await CrearFormulario());
            CargarPreguntasCommand = new Command(async () => await CargarPreguntas());

            // Cargar preguntas al iniciar
            Task.Run(async () => await CargarPreguntas());
        }

        public ObservableCollection<PreguntaViewModel> PreguntasDtos
        {
            get => _preguntasDtos;
            set
            {
                _preguntasDtos = value;
                OnPropertyChanged(nameof(PreguntasDtos));
            }
        }

        /// <summary>
        /// Carga las preguntas desde la API y las almacena en la colección PreguntasDtos.
        /// </summary>
        private async Task CargarPreguntas()
        {
            try
            {
                var preguntas = await _apiServicePregunta.GetPreguntas();

                if (preguntas == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No se pudieron obtener las preguntas. Verifica tu conexión o intenta nuevamente.", "OK");
                    return;
                }

                PreguntasDtos.Clear(); // Limpiar antes de cargar nuevas preguntas

                foreach (var pregunta in preguntas)
                {
                    var opciones = await _apiServiceOpcionRespuesta.GetOpcionRespuestaById(pregunta.IdPregunta) ?? new List<OpcionRespuestaDto>();

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

                    PreguntasDtos.Add(new PreguntaViewModel
                    {
                        PreguntaId = pregunta.IdPregunta,
                        TextoPregunta = pregunta.TextoPregunta,
                        Opciones = opcionesViewModel
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cargar las preguntas: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Actualiza la lista de preguntas seleccionadas para el formulario.
        /// </summary>
        public void ActualizarPreguntasSeleccionadas(PreguntaViewModel pregunta)
        {
            if (pregunta == null) return;

            if (pregunta.IsSelected)
            {
                if (!PreguntasSeleccionadas.Contains(pregunta))
                {
                    PreguntasSeleccionadas.Add(pregunta);
                }
            }
            else
            {
                PreguntasSeleccionadas.Remove(pregunta);
            }

            OnPropertyChanged(nameof(PreguntasSeleccionadas));
        }

        /// <summary>
        /// Crea un nuevo formulario con las preguntas seleccionadas.
        /// </summary>
        public async Task CrearFormulario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Titulo) || string.IsNullOrWhiteSpace(Descripcion))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El título y la descripción no pueden estar vacíos", "OK");
                    return;
                }

                if (PreguntasSeleccionadas.Count() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar al menos una pregunta para crear el formulario", "OK");
                    return;
                }

                await Application.Current.MainPage.DisplayAlert("Éxito", "Formulario guardado correctamente", "OK");
                await _navigation.PushAsync(new GestionFormularios());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al guardar el formulario: {ex.Message}", "OK");
            }
        }

        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set => SetProperty(ref _fechaInicio, value);
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set => SetProperty(ref _fechaFin, value);
        }

        public bool Habilitado
        {
            get => _habilitado;
            set => SetProperty(ref _habilitado, value);
        }

        public bool RequiereDatosPersonales
        {
            get => _requiereDatosPersonales;
            set => SetProperty(ref _requiereDatosPersonales, value);
        }
    } // Fin de la clase CrearFormularioViewModel
} // Fin del namespace

public class PreguntaViewModel
{
    public int PreguntaId { get; set; }
    public string TextoPregunta { get; set; }
    public ObservableCollection<OpcionRespuestaViewModel> Opciones { get; set; } = new ObservableCollection<OpcionRespuestaViewModel>();

    // ✅ Nueva propiedad para indicar si la pregunta está seleccionada

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }

    // ✅ Command para manejar la selección de preguntas
    public ICommand SeleccionarPreguntaCommand { get; }

    public PreguntaViewModel()
    {
        SeleccionarPreguntaCommand = new Command(() => IsSelected = !IsSelected);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



}

public class OpcionRespuestaViewModel
{
    public int OpcionId { get; set; }
    public string NombreOpcion { get; set; }
    public int IdPregunta { get; set; }
    public bool IsSelected { get; set; }
}

