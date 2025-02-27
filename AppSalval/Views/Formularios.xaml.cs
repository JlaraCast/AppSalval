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
        // Declaración de servicios para formularios y respuestas
        private readonly ApiServiceFormularios _apiService;
        private readonly ApiServiceRespuestas _apiServiceRespuestas; // ✅ Se agregó la instancia del servicio de respuestas
        private List<FormularioDto> _formularios;
        private List<RespuestasDTO> _respuestas; // ✅ Se declaró la lista de respuestas correctamente

        // Constructor de la clase
        public Formularios()
        {
            InitializeComponent();
            _apiService = new ApiServiceFormularios();
            _apiServiceRespuestas = new ApiServiceRespuestas(); // ✅ Se inicializa el servicio de respuestas
            LoadFormularios(); // Cargar los formularios al inicializar
        }

        // Método para cargar los formularios desde la API
        private async void LoadFormularios()
        {
            try
            {
                _formularios = await _apiService.GetFormularios(); // Obtener formularios desde la API

                // Filtrar formularios habilitados
                var formulariosHabilitados = _formularios
                    .Where(f => f.Habilitado)
                    .ToList();

                // Verificar si hay formularios habilitados
                if (formulariosHabilitados != null && formulariosHabilitados.Count > 0)
                {
                    // Asignar los títulos de los formularios habilitados al Picker
                    FormularioPicker.ItemsSource = formulariosHabilitados
                        .Select(f => f.TituloFormulario)
                        .ToList();
                }
                else
                {
                    // Mostrar alerta si no hay formularios habilitados
                    await DisplayAlert("Información", "No hay formularios habilitados disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores al cargar formularios
                await DisplayAlert("Error", $"Error al cargar formularios: {ex.Message}", "OK");
            }
        }

        // Método para buscar formularios según el texto ingresado en el cuadro de búsqueda
        //private void SearchFormularios(object sender, EventArgs e)
        //{
        //    string query = SearchBox.Text?.ToLower();
        //    if (!string.IsNullOrWhiteSpace(query))
        //    {
        //        // Filtrar formularios según el texto de búsqueda
        //        FormularioPicker.ItemsSource = _formularios
        //            .Where(f => f.TituloFormulario.ToLower().Contains(query))
        //            .Select(f => f.TituloFormulario)
        //            .ToList();
        //    }
        //}

        // Método que se ejecuta cuando se selecciona un formulario en el Picker
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

                    // Filtrar respuestas correspondientes al formulario seleccionado
                    var respuestasFiltradas = _respuestas
                        .Where(r => r.IdFormulario == formulario.IdFormulario)
                        .ToList();

                    Debug.WriteLine($"🔹 Total de respuestas filtradas para el formulario {formulario.TituloFormulario}: {respuestasFiltradas.Count}");

                    // Mostrar detalles de cada respuesta filtrada
                    foreach (var respuesta in respuestasFiltradas)
                    {
                        string identificacion = respuesta.IdentificacionEncuestado ?? "Sin Identificación";
                        Debug.WriteLine($"📝 Encuestado: {identificacion} - Fecha: {respuesta.FechaRespuesta:dd/MM/yyyy HH:mm:ss}");
                    }

                    // Asignar las respuestas filtradas a la vista
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
                    // Manejo de errores al cargar respuestas
                    await DisplayAlert("Error", $"Error al cargar respuestas: {ex.Message}", "OK");
                }
            }
        }

        // Método que se ejecuta al hacer clic en el botón "Aplicar"
        private async void OnAplicarClicked(object sender, EventArgs e)
        {
            if (FormularioPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Seleccione un formulario antes de continuar.", "OK");
                return;
            }

            // Obtener el título del formulario seleccionado
            string tituloSeleccionado = FormularioPicker.Items[FormularioPicker.SelectedIndex];
            FormularioDto formulario = _formularios.FirstOrDefault(f => f.TituloFormulario == tituloSeleccionado);

            if (formulario != null)
            {
                if (formulario.Anonimo)
                {
                    // Si el formulario es anónimo, ir directamente a AplicarFormulario.xaml
                    await Navigation.PushAsync(new AplicarFormulario(formulario.IdFormulario, formulario.TituloFormulario));
                }
                else
                {
                    // Si no es anónimo, primero pedir datos personales en InfoPersonal.xaml
                    await Navigation.PushAsync(new InfoPersonal(formulario.IdFormulario, formulario.TituloFormulario));
                }
            }
        }
    }
}
