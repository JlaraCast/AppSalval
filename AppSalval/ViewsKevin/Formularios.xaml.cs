using AppSalval.Models_Api;
using AppSaval.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views // CORREGIDO
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService; // Servicio para conectar con la API

        public MainPage()
        {
            InitializeComponent(); // Asegura que la página XAML se inicialice correctamente
            _apiService = new ApiService();
            LoadFormularios(); // Llamamos a la API cuando se carga la página
        }

        private async void LoadFormularios()
        {
            try
            {
                List<FormularioDto> formularios = await _apiService.GetFormularios();

                if (formularios != null && formularios.Count > 0)
                {
                    // Agregamos un número a cada formulario y concatenamos nombre + descripción
                    var formulariosConFormato = formularios
                        .Select((f, index) => new
                        {
                            Numero = $"{index + 1}.", // Enumeración
                            NombreDescripcion = $"{f.TituloFormulario} - {f.DescripcionFormulario}"
                        })
                        .ToList();

                    ListaFormularios.ItemsSource = formulariosConFormato;
                }
                else
                {
                    await DisplayAlert("Información", "No hay formularios disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar los formularios: {ex.Message}", "OK");
            }
        }

    }
}