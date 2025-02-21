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

namespace AppSalval.ViewModels
{
    public class VerFormularioCreadoViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private FormularioDto _formulario;
        private String _titulo;
        private String _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _habilitado;
        private bool _requiereDatosPersonales;
        private ICommand _btnRegresar;

        private readonly ApiServiceFormularioPregunta _apiServiceFormulario;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcion;
        

        private List<FormularioPreguntaDto> _preguntas;
        private CollectionView ListaPreguntas;

        public VerFormularioCreadoViewModel(INavigation navigation, FormularioDto formulario, CollectionView listaPreguntas)
        {
            _apiServiceFormulario = new ApiServiceFormularioPregunta();
            _apiServiceOpcion = new ApiServiceOpcionRespuesta();
            ListaPreguntas = listaPreguntas;

            _navigation = navigation;
            _formulario = formulario;
            Titulo = formulario.TituloFormulario;
            Descripcion = formulario.DescripcionFormulario;
            FechaInicio = formulario.FechaInicio;
            FechaFin = formulario.FechaFin;
            Habilitado = formulario.Habilitado;
            RequiereDatosPersonales = !formulario.Anonimo;

            LoadPreguntas(formulario.IdFormulario);
            
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
        public ICommand BtnRegresar => _btnRegresar ??= new Command(ComandoBtnRegresar);


        private async void ComandoBtnRegresar()
        {
            await _navigation.PushAsync(new GestionFormularios());
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

                    ListaPreguntas.ItemsSource = _preguntas;
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

    }
}
