using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class CrearFormularioViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly ApiServicePregunta _apiServicePregunta;
        private readonly ApiServiceFormularios _apiServiceFormularios;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcionRespuesta;
        private readonly ApiServiceReglaOpcion _apiServiceReglaOpcion;
        private CollectionView _listaPreguntas;

        private string _titulo;
        private string _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _requiereDatosPersonales;
        private bool _habilitado;
        private bool _checkboxPregunta;
        private ObservableCollection<OpcionRespuestaDtoExtendida> _opcionesRespuesta;
        private int _preguntaId = -1;

        private ObservableCollection<PreguntaDto> _preguntasDtos;




        public CrearFormularioViewModel(CollectionView listaPreguntas)
        {
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceFormularios = new ApiServiceFormularios();
            _apiServiceOpcionRespuesta = new ApiServiceOpcionRespuesta();
            _apiServiceReglaOpcion = new ApiServiceReglaOpcion();
            this._listaPreguntas = listaPreguntas;

            _titulo = string.Empty;
            _descripcion = string.Empty;
            _opcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>();
           
            _fechaInicio = DateTime.Now;
            _fechaFin = DateTime.Now;
            _requiereDatosPersonales = false;
            _habilitado = true;
            _checkboxPregunta = false;



            BtnCancelar = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionFormularios());
            });

            BtnAceptar = new Command(async () =>
            {
                // Lógica para aceptar el formulario
            });

        }


        
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

        public CrearFormularioViewModel()
        {
            _apiServicePregunta = new ApiServicePregunta();
            _apiServiceFormularios = new ApiServiceFormularios();
            _apiServiceOpcionRespuesta = new ApiServiceOpcionRespuesta();
            _apiServiceReglaOpcion = new ApiServiceReglaOpcion();
            _titulo = string.Empty;
            _descripcion = string.Empty;
            _fechaInicio = DateTime.Now;
            _fechaFin = DateTime.Now;
            _requiereDatosPersonales = false;
            _habilitado = true;
            _checkboxPregunta = false;
            OpcionesRespuesta = new ObservableCollection<OpcionRespuestaDtoExtendida>();
            

            BtnCancelar = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionFormularios());
            });

            BtnAceptar = new Command(async () =>
            {
                // Lógica para aceptar el formulario
            });

            _preguntasDtos = new ObservableCollection<PreguntaDto>(); // Asegurar que está inicializado

            CargarPreguntasCommand = new Command(async () => await CargarPreguntas());

            // Cargar preguntas al iniciar
            Task.Run(async () => await CargarPreguntas());
        }

        public ObservableCollection<PreguntaDto> PreguntasDtos
        {
            get => _preguntasDtos;
            set
            {
                _preguntasDtos = value;
                OnPropertyChanged(nameof(PreguntasDtos)); // ✅ Notificar cambios a la UI
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

            PreguntasDtos.Clear(); // Limpiar antes de agregar nuevas preguntas

            foreach (var pregunta in preguntas)
            {
                PreguntasDtos.Add(pregunta);
            }
        }



        public ICommand BtnCancelar { get; }
        public ICommand BtnAceptar { get; }

        public ICommand CargarOpcionesRespuestaCommand { get; }

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

        public bool CheckboxPregunta
        {
            get => _checkboxPregunta;
            set => SetProperty(ref _checkboxPregunta, value);
        }
      
        


        








        internal class PreguntaViewModel
        {
            public int PreguntaId { get; set; }
            public string TextoPregunta { get; set; }
            public string TipoPregunta { get; set; }
            public string NombreOpciones { get; set; }
            public Command VerCommand { get; set; }
            public Command EditarCommand { get; set; }
            public Command EliminarCommand { get; set; }
        }

        internal class OpcionRespuestaViewModel
        {
            public int OpcionId { get; set; }
            public string NombreOpcion { get; set; }
            public int IdPregunta { get; set; }
            public bool IsSelected { get; set; }
        }



    }//fin de la clase

}//Fin del Namespace
