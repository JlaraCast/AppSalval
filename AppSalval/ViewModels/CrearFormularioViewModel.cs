﻿using System;
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
using static AppSalval.ViewModels.CrearFormularioViewModel;


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

        public ObservableCollection<PreguntaViewModel> _preguntasDtos { get; set; }

        public ObservableCollection<PreguntaViewModel> PreguntasSeleccionadas { get; set; } = new ObservableCollection<PreguntaViewModel>();





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

        public void ActualizarPreguntasSeleccionadas()
        {
            PreguntasSeleccionadas.Clear();
            foreach (var pregunta in PreguntasDtos)
            {
                if (pregunta.IsSelected)
                {
                    PreguntasSeleccionadas.Add(pregunta);
                }
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

            _preguntasDtos = new ObservableCollection<PreguntaViewModel>(); // Asegurar que está inicializado

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
                OnPropertyChanged(nameof(PreguntasDtos)); // 🔄 Notifica cambios a la UI
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


    }//fin de la clase


    public class PreguntaViewModel
{
    public int PreguntaId { get; set; }
    public string TextoPregunta { get; set; }
    public ObservableCollection<OpcionRespuestaViewModel> Opciones { get; set; } = new ObservableCollection<OpcionRespuestaViewModel>();

    // ✅ Nueva propiedad para indicar si la pregunta está seleccionada
    public bool IsSelected { get; set; }
}


    public class OpcionRespuestaViewModel
    {
        public int OpcionId { get; set; }
        public string NombreOpcion { get; set; }
        public int IdPregunta { get; set; }
        public bool IsSelected { get; set; }
    }



}//Fin del Namespace
