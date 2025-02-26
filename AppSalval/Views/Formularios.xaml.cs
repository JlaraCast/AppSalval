using AppSalval.Models_Api;
using AppSalval.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using AppSalval.DTOS_API; // ✅ Se agregó la referencia para RespuestaDto

namespace AppSalval.Views
{
    public partial class Formularios : ContentPage
    {
        private readonly ApiServiceFormularios _apiService;
        private readonly ApiServiceRespuestas _apiServiceRespuestas; // ✅ Se agregó la instancia del servicio de respuestas
        private readonly ApiServiceFormularioPregunta _apiServicePreguntas;
        private List<FormularioDto> _formularios;
        private List<RespuestasDTO> _respuestas; // ✅ Se declaró la lista de respuestas correctamente

        public Formularios()
        {
            InitializeComponent();
            _apiService = new ApiServiceFormularios();
            _apiServicePreguntas = new ApiServiceFormularioPregunta();
            _apiServiceRespuestas = new ApiServiceRespuestas(); // ✅ Se inicializa el servicio de respuestas
            LoadFormularios();
        }

        private async void LoadFormularios()
        {
            try
            {
                _formularios = await _apiService.GetFormularios();

                if (_formularios == null || _formularios.Count == 0)
                {
                    await DisplayAlert("Información", "No hay formularios disponibles", "OK");
                    return;
                }

                var formulariosConPreguntas = new List<string>();

                foreach (var formulario in _formularios.Where(f => f.Habilitado))
                {
                    var preguntas = await _apiServicePreguntas.GetPreguntasByFormulario(formulario.IdFormulario);

                    if (preguntas != null && preguntas.Count > 0)
                    {
                        formulariosConPreguntas.Add(formulario.TituloFormulario);
                        Debug.WriteLine($"✅ Formulario '{formulario.TituloFormulario}' tiene {preguntas.Count} preguntas.");
                    }
                    else
                    {
                        Debug.WriteLine($"❌ Formulario '{formulario.TituloFormulario}' no tiene preguntas y será excluido.");
                    }
                }

                if (formulariosConPreguntas.Count > 0)
                {
                    FormularioPicker.ItemsSource = formulariosConPreguntas;
                }
                else
                {
                    await DisplayAlert("Información", "No hay formularios con preguntas disponibles", "OK");
                    FormularioPicker.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar formularios: {ex.Message}", "OK");
            }
        }
    

        private void SearchFormularios(object sender, EventArgs e)
        {
            string query = SearchBox.Text?.ToLower();
            if (!string.IsNullOrWhiteSpace(query))
            {
                FormularioPicker.ItemsSource = _formularios
                    .Where(f => f.TituloFormulario.ToLower().Contains(query))
                    .Select(f => f.TituloFormulario)
                    .ToList();
            }
        }

        private async void FormularioPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormularioPicker.SelectedIndex == -1)
                return;

            // Obtener el título del formulario seleccionado
            string tituloSeleccionado = FormularioPicker.Items[FormularioPicker.SelectedIndex];

            // Buscar el formulario correspondiente en la lista de formularios
            FormularioDto formulario = _formularios.FirstOrDefault(f => f.TituloFormulario == tituloSeleccionado);

            if (formulario != null)
            {
                Debug.WriteLine($"📌 Cargando respuestas para el formulario: {formulario.TituloFormulario}");

                try
                {
                    // Cargar respuestas asociadas al formulario
                    _respuestas = await _apiServiceRespuestas.GetRespuestas();
                    Debug.WriteLine($"✅ Total de respuestas obtenidas de la API: {_respuestas.Count}");

                    var respuestasFiltradas = _respuestas
                        .Where(r => r.IdFormulario == formulario.IdFormulario)
                        .ToList();

                    Debug.WriteLine($"🔹 Total de respuestas filtradas para el formulario {formulario.TituloFormulario}: {respuestasFiltradas.Count}");

                    foreach (var respuesta in respuestasFiltradas)
                    {
                        string identificacion = respuesta.IdentificacionEncuestado ?? "Sin Identificación";
                        Debug.WriteLine($"📝 Encuestado: {identificacion} - Fecha: {respuesta.FechaRespuesta:dd/MM/yyyy HH:mm:ss}");
                    }

                    if (respuestasFiltradas.Count > 0)
                    {
                        ListaRespuestas.ItemsSource = null; // Limpiar la vista antes de asignar nuevos datos
                        ListaRespuestas.ItemsSource = respuestasFiltradas;
                    }
                    else
                    {
                        ListaRespuestas.ItemsSource = null;
                        await DisplayAlert("Información", "No hay respuestas para este formulario", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al cargar respuestas: {ex.Message}", "OK");
                }
            }
        }

        private async void OnAplicarClicked(object sender, EventArgs e)
        {
            if (FormularioPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Seleccione un formulario antes de continuar.", "OK");
                return;
            }

            string tituloSeleccionado = FormularioPicker.Items[FormularioPicker.SelectedIndex];
            FormularioDto formulario = _formularios.FirstOrDefault(f => f.TituloFormulario == tituloSeleccionado);

            if (formulario != null)
            {
                // ✅ Manteniendo funcionalidad original: Navegar según el formulario sea anónimo o no
                if (formulario.Anonimo)
                {
                    await Navigation.PushAsync(new AplicarFormulario(formulario.IdFormulario, formulario.TituloFormulario));
                }
                else
                {
                    await Navigation.PushAsync(new InfoPersonal(formulario.IdFormulario, formulario.TituloFormulario));
                }

                // ✅ Nueva funcionalidad: Guardar respuesta en la API
                var nuevaRespuesta = new RespuestasDTO
                (
                    idRespuesta: 0, // Se generará automáticamente en la base de datos
                    idFormulario: formulario.IdFormulario,
                    identificacionEncuestado: formulario.Anonimo ? null : "12345678", // Si es anónimo, no se envía ID (esto deberá adaptarse a la identificación real)
                    fechaRespuesta: DateTime.Now
                );

                bool respuestaGuardada = await _apiServiceRespuestas.SaveRespuesta(nuevaRespuesta);

                if (respuestaGuardada)
                {
                    await DisplayAlert("Éxito", "La respuesta se ha guardado correctamente.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo guardar la respuesta. Intente nuevamente.", "OK");
                }
            }
        }


    }
}
