using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Models_Api;
using AppSalval.Services;
using AppSalval.Views;
using AppSalval.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppSalval.ViewModels
{
    public class EditarFormularioViewModel : BaseViewModel
    {
        // Campos privados
        private INavigation _navigation;
        private FormularioDto _formulario;
        private String _titulo;
        private String _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _habilitado;
        private bool _requiereDatosPersonales;

        // Servicios API
        private readonly ApiServiceFormularioPregunta _apiServiceFormulario;
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcion;
        private readonly ApiServiceFormularios _apiFormulario;

        // Colecciones observables
        public ObservableCollection<PreguntaViewModel> _preguntasDtos { get; set; }
        public ObservableCollection<PreguntaViewModel> PreguntasSeleccionadas { get; set; } = new ObservableCollection<PreguntaViewModel>();

        // Comandos
        public ICommand BtnRegresar { get; }
        public ICommand BtnGuardar { get; }
        public ICommand ActualizarPreguntasSeleccionadasCommand { get; }

        // Lista de preguntas
        private List<FormularioPreguntaDto> _preguntas;

        // Constructor
        public EditarFormularioViewModel(INavigation navigation, FormularioDto formulario)
        {
            // Inicialización de servicios API
            _apiServiceFormulario = new ApiServiceFormularioPregunta();
            _apiServiceOpcion = new ApiServiceOpcionRespuesta();
            _apiFormulario = new ApiServiceFormularios();
            _apiServicePregunta = new ApiServicePregunta();

            // Inicialización de comandos
            ActualizarPreguntasSeleccionadasCommand = new Command<PreguntaViewModel>(ActualizarPreguntasSeleccionadas);

            // Asignación de valores iniciales
            _navigation = navigation;
            _formulario = formulario;
            Titulo = formulario.TituloFormulario;
            Descripcion = formulario.DescripcionFormulario;
            FechaInicio = formulario.FechaInicio;
            FechaFin = formulario.FechaFin;
            Habilitado = formulario.Habilitado;
            RequiereDatosPersonales = !formulario.Anonimo;

            // Inicialización de colecciones
            PreguntasDtos = new ObservableCollection<PreguntaViewModel>();

            // Cargar preguntas del formulario
            CargarPreguntas(formulario);

            // Inicialización de comandos de botones
            BtnRegresar = new Command(ComandoBtnRegresar);
            BtnGuardar = new Command(async () => await GuardarCambiosFormulario());
        }

        // Propiedades con notificación de cambios
        public String Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        public String Descripcion
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

        // Comando para regresar a la vista anterior
        private async void ComandoBtnRegresar()
        {
            await _navigation.PushAsync(new GestionFormularios());
        }

        // Propiedad para la colección de preguntas
        public ObservableCollection<PreguntaViewModel> PreguntasDtos
        {
            get => _preguntasDtos;
            set
            {
                _preguntasDtos = value;
                OnPropertyChanged(nameof(PreguntasDtos)); // Notifica cambios a la UI
            }
        }

        // Método para actualizar la lista de preguntas seleccionadas
        public void ActualizarPreguntasSeleccionadas(PreguntaViewModel pregunta)
        {
            if (pregunta == null) return;

            if (pregunta.IsSelected)
            {
                if (!PreguntasSeleccionadas.Contains(pregunta))
                {
                    PreguntasSeleccionadas.Add(pregunta);
                    Console.WriteLine($"Pregunta agregada: {pregunta.PreguntaId}");
                }
            }
            else
            {
                PreguntasSeleccionadas.Remove(pregunta);
                Console.WriteLine($"Pregunta removida: {pregunta.PreguntaId}");
            }

            OnPropertyChanged(nameof(PreguntasSeleccionadas));
        }

        // Método para cargar preguntas del formulario
        private async Task CargarPreguntas(FormularioDto formulario)
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
                var opciones = await _apiServiceOpcion.GetOpcionRespuestaById(pregunta.IdPregunta) ?? new List<OpcionRespuestaDto>();

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
                var preguntaViewModel = new PreguntaViewModel
                {
                    PreguntaId = pregunta.IdPregunta,
                    TextoPregunta = pregunta.TextoPregunta,
                    Opciones = opcionesViewModel
                };

                _preguntas = await _apiServiceFormulario.GetPreguntasByFormulario(formulario.IdFormulario);

                // Marcar la pregunta como seleccionada si está en el formulario
                if (_preguntas.Any(p => p.IdPregunta == pregunta.IdPregunta))
                {
                    preguntaViewModel.IsSelected = true;
                    ActualizarPreguntasSeleccionadas(preguntaViewModel);
                }

                PreguntasDtos.Add(preguntaViewModel);
            }
        }

        // Método para guardar cambios en el formulario
        private async Task GuardarCambiosFormulario()
        {
            try
            {
                // Actualizar propiedades del formulario
                _formulario.TituloFormulario = Titulo;
                _formulario.DescripcionFormulario = Descripcion;
                _formulario.FechaInicio = FechaInicio;
                _formulario.FechaFin = FechaFin;
                _formulario.Habilitado = Habilitado;
                _formulario.Anonimo = !RequiereDatosPersonales;

                // Guardar cambios a través del servicio API
                bool resultado = await _apiFormulario.EditFormulario(_formulario);

                if (resultado)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Formulario guardado correctamente.", "OK");
                    await _navigation.PushAsync(new GestionFormularios());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar el formulario.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al guardar el formulario: {ex.Message}", "OK");
            }
        }
    }


}

    