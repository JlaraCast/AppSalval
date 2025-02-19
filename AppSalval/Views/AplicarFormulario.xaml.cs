using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using AppSalval.Services;
using AppSalval.DTOS_API;

namespace AppSalval.Views
{
    public partial class AplicarFormulario : ContentPage
    {
        private readonly ApiServiceFormularioPregunta _apiServiceFormulario;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcion;
        private List<FormularioPreguntaIdDto> _preguntas;

        public AplicarFormulario(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _apiServiceFormulario = new ApiServiceFormularioPregunta();
            _apiServiceOpcion = new ApiServiceOpcionRespuesta();
            FormularioTitulo.Text = tituloFormulario;

            Debug.WriteLine("📌 Constructor de AplicarFormulario ejecutado");

            LoadPreguntas(idFormulario);
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
