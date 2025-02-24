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
        private readonly RecomService _apiServiceRecomendacion;
        private readonly FactorService _apiServiceFactor;
        private readonly ApiServiceOpcionRespuesta _apiServiceOpcion;
        private readonly ApiServiceReglaOpcion _apiServiceReglaOpcion;
        private List<FormularioPreguntaDto> _preguntas;

        public AplicarFormulario(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _apiServiceFormulario = new ApiServiceFormularioPregunta();
            _apiServiceRecomendacion = new RecomService(); // ✅ Asegurar inicialización del servicio
            _apiServiceFactor = new FactorService(); // ✅ Si también usas factores, inicialízalo
            FormularioTitulo.Text = tituloFormulario;
            _apiServiceOpcion = new ApiServiceOpcionRespuesta();
            _apiServiceReglaOpcion = new ApiServiceReglaOpcion();
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
                        Debug.WriteLine($"✅ Pregunta ID: {pregunta.IdPregunta}, Tipo: {pregunta.TipoPregunta}");

                        var opciones = await _apiServiceOpcion.GetValidOpcionRespuestasByPreguntaId(pregunta.IdPregunta);

                        if (opciones != null && opciones.Count > 0)
                        {
                            foreach (var opcion in opciones)
                            {
                                opcion.TipoPregunta = pregunta.TipoPregunta;
                            }

                            pregunta.OpcionesRespuesta = opciones;
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
            await ObtenerRecomendacionesYFactores();
        }



        private async void OnOpcionSeleccionada(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value) // Solo si se seleccionó (Checked = true)
            {
                var radioButton = sender as RadioButton;
                if (radioButton != null && radioButton.BindingContext is OpcionRespuestaDto opcionSeleccionada)
                {
                    Debug.WriteLine($"✅ Opción seleccionada: {opcionSeleccionada.NombreOpcion} (ID: {opcionSeleccionada.IdOpcion})");

                    // Cargar factores de riesgo y recomendaciones para la opción seleccionada
                    await CargarFactoresYRecomendaciones(opcionSeleccionada.IdOpcion);
                }
            }
        }

        private async Task CargarFactoresYRecomendaciones(int idOpcion)
        {
            try
            {
                var reglas = await _apiServiceReglaOpcion.GetReglaOpcionByOpcionId(idOpcion);

                if (reglas != null && reglas.Count > 0)
                {
                    List<string> recomendaciones = new List<string>();
                    List<string> factoresRiesgo = new List<string>();

                    foreach (var regla in reglas)
                    {
                        if (regla.IdRecomendacion.HasValue)
                        {
                            var recomendacion = await _apiServiceRecomendacion.GetRecomendacionByIdAsync(regla.IdRecomendacion.Value);
                            if (recomendacion != null)
                            {
                                recomendaciones.Add(recomendacion.TextoRecomendacion);
                            }
                        }

                        if (regla.IdFactorRiesgo.HasValue)
                        {
                            var factor = await _apiServiceFactor.GetFactorByIdAsync(regla.IdFactorRiesgo.Value);
                            if (factor != null)
                            {
                                factoresRiesgo.Add(factor.TextoFactor);
                            }
                        }
                    }

                    Debug.WriteLine($"🔹 Recomendaciones cargadas: {recomendaciones.Count}");
                    Debug.WriteLine($"🔹 Factores de riesgo cargados: {factoresRiesgo.Count}");

                    // Pasar los datos a ResultadosUsuario
                    await Navigation.PushAsync(new ResultadosUsuario(recomendaciones, factoresRiesgo));
                }
                else
                {
                    Debug.WriteLine("⚠️ No se encontraron reglas asociadas a esta opción.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error en CargarFactoresYRecomendaciones: {ex.Message}");
            }
        }


        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async Task ObtenerRecomendacionesYFactores()
        {
            var recomendaciones = new List<string>();
            var factoresRiesgo = new List<string>();

            foreach (var pregunta in _preguntas)
            {
                List<OpcionRespuestaDto> opcionesSeleccionadas = new List<OpcionRespuestaDto>();

                // ✅ Para preguntas de selección múltiple (CheckBox)
                if (pregunta.TipoPregunta.ToLower().Contains("multiple"))
                {
                    opcionesSeleccionadas = pregunta.OpcionesRespuesta
                        .Where(op => op.IsSelected) // Solo las opciones marcadas
                        .ToList();
                }
                // ✅ Para preguntas de selección única (RadioButton)
                else if (pregunta.TipoPregunta.ToLower().Contains("única") || pregunta.TipoPregunta.ToLower().Contains("unica"))
                {
                    opcionesSeleccionadas = pregunta.OpcionesRespuesta
                        .Where(op => op.IsSelected) // Solo la opción seleccionada
                        .Take(1) // Garantiza que solo una opción sea tomada
                        .ToList();
                }

                // 🔹 Ahora procesamos solo las opciones seleccionadas
                foreach (var opcion in opcionesSeleccionadas)
                {
                    var reglas = await _apiServiceReglaOpcion.GetReglaOpcionByOpcionId(opcion.IdOpcion);

                    if (reglas != null)
                    {
                        foreach (var regla in reglas)
                        {
                            if (regla.IdRecomendacion.HasValue)
                            {
                                var recomendacion = await _apiServiceRecomendacion.GetRecomendacionByIdAsync(regla.IdRecomendacion.Value);
                                if (recomendacion != null && !recomendaciones.Contains(recomendacion.TextoRecomendacion))
                                {
                                    recomendaciones.Add(recomendacion.TextoRecomendacion);
                                }
                            }

                            if (regla.IdFactorRiesgo.HasValue)
                            {
                                var factorRiesgo = await _apiServiceFactor.GetFactorByIdAsync(regla.IdFactorRiesgo.Value);
                                if (factorRiesgo != null && !factoresRiesgo.Contains(factorRiesgo.TextoFactor))
                                {
                                    factoresRiesgo.Add(factorRiesgo.TextoFactor);
                                }
                            }
                        }
                    }
                }
            }

            // ✅ Solo después de procesar la información, navegamos a la pantalla de resultados
            await Navigation.PushAsync(new ResultadosUsuario(recomendaciones, factoresRiesgo));
        }

    }
}
