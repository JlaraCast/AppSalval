using AppSalval.Models_Api;
using AppSalval.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class Formularios : ContentPage
    {
        private readonly ApiServiceFormularios _apiService;
        private List<FormularioDto> _formularios;

        public Formularios()
        {
            InitializeComponent();
            _apiService = new ApiServiceFormularios();
            LoadFormularios();
        }

        private async void LoadFormularios()
        {
            try
            {
                _formularios = await _apiService.GetFormularios();

                // 🔹 Filtrar formularios que tienen habilitado = true
                var formulariosHabilitados = _formularios
                    .Where(f => f.Habilitado) // Solo los habilitados
                    .ToList();

                if (formulariosHabilitados != null && formulariosHabilitados.Count > 0)
                {
                    FormularioPicker.ItemsSource = formulariosHabilitados
                        .Select(f => f.TituloFormulario)
                        .ToList();
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

        private void FormularioPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormularioPicker.SelectedIndex == -1)
                return;

            string tituloSeleccionado = FormularioPicker.Items[FormularioPicker.SelectedIndex];
            FormularioDto formulario = _formularios.FirstOrDefault(f => f.TituloFormulario == tituloSeleccionado);

            if (formulario != null)
            {
                DisplayAlert("Formulario Seleccionado", $"Seleccionaste: {formulario.TituloFormulario}", "OK");
            }
        }
    }
}
