using AppSalval.Services;
using AppSalval.DTOS_API;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class AplicarFormulario : ContentPage
    {
        private readonly ApiServiceFormularioPregunta _apiServiceFormularioPregunta;
        private readonly ApiServiceOpcionesRespuesta _apiServiceOpcionesRespuesta;
        private List<FormularioPreguntaDto> _preguntas;

        public AplicarFormulario(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _apiServiceFormularioPregunta = new ApiServiceFormularioPregunta();
            _apiServiceOpcionesRespuesta = new ApiServiceOpcionesRespuesta();
            FormularioTitulo.Text = tituloFormulario;
            LoadPreguntas(idFormulario);
        }

        private async void LoadPreguntas(int idFormulario)
        {
            try
            {
                _preguntas = await _apiServiceFormularioPregunta.GetPreguntasByFormulario(idFormulario);

                if (_preguntas != null && _preguntas.Count > 0)
                {
                    foreach (var pregunta in _preguntas)
                    {
                        pregunta.Opciones = await _apiServiceOpcionesRespuesta.GetOpcionesByPregunta(pregunta.IdPregunta);
                    }

                    ListaPreguntas.ItemsSource = _preguntas;
                }
                else
                {
                    await DisplayAlert("Información", "No hay preguntas en este formulario.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar preguntas: {ex.Message}", "OK");
            }
        }

        private async void OnEnviarClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Éxito", "Respuestas enviadas correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
