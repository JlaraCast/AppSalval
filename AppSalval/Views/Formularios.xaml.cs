using AppSalval.Models_Api;
using AppSalval.Services;
using AppSalval.DTOS_API; // 🔹 Importación para reconocer RespuestasDTO
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class Formularios : ContentPage
    {
        private readonly ApiServiceFormularios _apiServiceFormularios;
        private readonly ApiServiceRespuestas _apiServiceRespuestas;
        private List<FormularioDto> _formularios;
        private List<RespuestasDTO> _respuestas;

        public Formularios()
        {
            InitializeComponent();
            _apiServiceFormularios = new ApiServiceFormularios();
            _apiServiceRespuestas = new ApiServiceRespuestas();
            LoadFormularios();
        }

        private async Task LoadFormularios()
        {
            try
            {
                _formularios = await _apiServiceFormularios.GetFormularios();
                var formulariosHabilitados = _formularios?.Where(f => f.Habilitado).ToList();

                if (formulariosHabilitados != null && formulariosHabilitados.Count > 0)
                {
                    FormularioPicker.ItemsSource = formulariosHabilitados.Select(f => f.TituloFormulario).ToList();
                }
                else
                {
                    await DisplayAlert("Información", "No hay formularios habilitados disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar formularios: {ex.Message}", "OK");
            }
        }

        private async void FormularioPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormularioPicker.SelectedIndex == -1)
                return;

            string tituloSeleccionado = FormularioPicker.Items[FormularioPicker.SelectedIndex];
            FormularioDto formulario = _formularios.FirstOrDefault(f => f.TituloFormulario == tituloSeleccionado);

            if (formulario != null)
            {
                await LoadRespuestas(formulario.IdFormulario);
            }
        }

        private async Task LoadRespuestas(int idFormulario)
        {
            try
            {
                _respuestas = await _apiServiceRespuestas.GetRespuestas();
                var respuestasFiltradas = _respuestas
                    .Where(r => r.IdFormulario == idFormulario)
                    .ToList();

                if (respuestasFiltradas.Count > 0)
                {
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

        private void SearchFormularios(object sender, EventArgs e)
        {
            string query = SearchBox.Text?.ToLower();
            if (!string.IsNullOrWhiteSpace(query) && _formularios != null)
            {
                FormularioPicker.ItemsSource = _formularios
                    .Where(f => f.TituloFormulario.ToLower().Contains(query))
                    .Select(f => f.TituloFormulario)
                    .ToList();
            }
        }

        private async void OnFormularioPickerFocused(object sender, FocusEventArgs e)
        {
            await LoadFormularios();
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
                await DisplayAlert("Aplicar", $"Formulario '{formulario.TituloFormulario}' seleccionado.", "OK");
                // FUTURAMENTE: Aquí podrías abrir una nueva página donde el usuario complete el formulario
            }
        }
    }
}
