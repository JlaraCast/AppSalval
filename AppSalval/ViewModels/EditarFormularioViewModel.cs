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

        private INavigation _navigation;
        private FormularioDto _formulario;
        private String _titulo;
        private String _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _habilitado;
        private bool _requiereDatosPersonales;
        


        private readonly ApiServiceFormularioPregunta _apiServiceFormulario;
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcion;
        private readonly ApiServiceFormularios _apiFormulario;
     

        public ObservableCollection<PreguntaViewModel> _preguntasDtos { get; set; }

        public ICommand BtnRegresar { get; }
        public ICommand BtnGuardar { get; }


        private List<FormularioPreguntaDto> _preguntas;
        

        public EditarFormularioViewModel(INavigation navigation, FormularioDto formulario)
        {
            _apiServiceFormulario = new ApiServiceFormularioPregunta();
            _apiServiceOpcion = new ApiServiceOpcionRespuesta();
            _apiFormulario = new ApiServiceFormularios();
            _apiServicePregunta = new ApiServicePregunta();
            

            _navigation = navigation;
            _formulario = formulario;
            Titulo = formulario.TituloFormulario;
            Descripcion = formulario.DescripcionFormulario;
            FechaInicio = formulario.FechaInicio;
            FechaFin = formulario.FechaFin;
            Habilitado = formulario.Habilitado;
            RequiereDatosPersonales = !formulario.Anonimo;

            PreguntasDtos = new ObservableCollection<PreguntaViewModel>();

            CargarPreguntas();

            BtnRegresar = new Command(ComandoBtnRegresar);
            BtnGuardar = new Command(async () => await GuardarCambiosFormulario());
        }

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
        
        private async void ComandoBtnRegresar()
        {
            await _navigation.PushAsync(new GestionFormularios());
        }

        public ObservableCollection<PreguntaViewModel> PreguntasDtos
        {
            get => _preguntasDtos;
            set
            {
                _preguntasDtos = value;
                OnPropertyChanged(nameof(PreguntasDtos)); // 🔄 Notifica cambios a la UI
            }
        }

        private async void LoadPreguntas(int idFormulario)
        {
            try
            {
                _preguntas = await _apiServiceFormulario.GetPreguntasByFormulario(idFormulario);

                if (_preguntas != null && _preguntas.Count > 0)
                {
                    foreach (var pregunta in _preguntas)
                    {
                        Debug.WriteLine($"✅ Pregunta ID: {pregunta.IdPregunta}, Texto: {pregunta.TextPregunta}");

                        // Obtener opciones válidas desde la API
                        var opciones = await _apiServiceOpcion.GetValidOpcionRespuestasByPreguntaId(pregunta.IdPregunta);

                        if (opciones != null && opciones.Count > 0)
                        {
                            Debug.WriteLine($"🔹 Opciones cargadas para la pregunta {pregunta.IdPregunta}: {opciones.Count}");
                            foreach (var opcion in opciones)
                            {
                                Debug.WriteLine($"   - Opción: {opcion.NombreOpcion}");
                            }
                            pregunta.OpcionesRespuesta = opciones;
                        }
                        else
                        {
                            Debug.WriteLine($"⚠️ No se encontraron opciones válidas para la pregunta {pregunta.IdPregunta}");
                        }
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay preguntas en este formulario.", "OK");

                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar preguntas: {ex.Message}", "OK");

            }
        }


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
                PreguntasDtos.Add(new PreguntaViewModel
                {
                    PreguntaId = pregunta.IdPregunta,
                    TextoPregunta = pregunta.TextoPregunta,
                    Opciones = opcionesViewModel
                });


            }
        }


        private async Task GuardarCambiosFormulario()
        {
            try
            {
                _formulario.TituloFormulario = Titulo;
                _formulario.DescripcionFormulario = Descripcion;
                _formulario.FechaInicio = FechaInicio;
                _formulario.FechaFin = FechaFin;
                _formulario.Habilitado = Habilitado;
                _formulario.Anonimo = !RequiereDatosPersonales;

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
