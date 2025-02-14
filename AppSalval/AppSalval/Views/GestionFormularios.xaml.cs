using AppSalval.Models_Api;
using AppSaval.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AppSalval.Views
{
    public partial class GestionFormularios : ContentPage
    {
        private readonly ApiServiceFormularios _apiService; // Servicio para conectar con la API

        public Command VerCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public GestionFormularios()
        {
            InitializeComponent();
            _apiService = new ApiServiceFormularios();
            VerCommand = new Command<FormularioDto>(OnVerFormularioClicked);
            EditarCommand = new Command<FormularioDto>(OnEditarFormularioClicked);
            EliminarCommand = new Command<FormularioDto>(OnEliminarFormularioClicked);
            LoadFormularios(); // Llamamos a la API cuando se carga la p�gina
        }

        private async void LoadFormularios()
        {
            try
            {
                List<FormularioDto> formularios = await _apiService.GetFormularios();

                if (formularios != null && formularios.Count > 0)
                {
                    // Filtramos los formularios habilitados y agregamos un n�mero a cada uno
                    var formulariosConFormato = formularios
                        .Where(f => f.Habilitado)
                        .Select(f => new
                        {
                            f.IdFormulario,
                            NombreDescripcion = $"{f.TituloFormulario} - {f.DescripcionFormulario}",
                            VerCommand = VerCommand,
                            EditarCommand = EditarCommand,
                            EliminarCommand = EliminarCommand
                        })
                        .ToList();

                    ListaFormularios.ItemsSource = formulariosConFormato;
                }
                else
                {
                    await DisplayAlert("Informaci�n", "No hay formularios disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurri� un error al cargar los formularios: {ex.Message}", "OK");
            }
        }

        private void OnOpcionesClicked(object sender, EventArgs e)
        {
            // L�gica para desplegar opciones
            Console.WriteLine("Opciones clicked.");
        }

        private void OnMenuPrincipalClicked(object sender, EventArgs e)
        {
            // L�gica para salir al men� principal
            Console.WriteLine("Men� Principal clicked.");
        }

        private void OnVerFormularioClicked(FormularioDto formulario)
        {
            // L�gica para ver el formulario
            Console.WriteLine($"Ver formulario: {formulario.TituloFormulario}");
        }

        private void OnEditarFormularioClicked(FormularioDto formulario)
        {
            // L�gica para editar el formulario
            Console.WriteLine($"Editar formulario: {formulario.TituloFormulario}");
        }

        private async void OnEliminarFormularioClicked(FormularioDto formulario)
        {
            // Buscar el formulario por su ID
            var formularioEncontrado = await _apiService.GetFormularioById(formulario.IdFormulario);

            if (formularioEncontrado != null)
            {
                // Deshabilitar el formulario
                formularioEncontrado.Habilitado = false;

                // Llamar al m�todo para actualizar la base de datos
                await _apiService.EditFormulario(formularioEncontrado);

                // Mostrar mensaje de confirmaci�n
                await DisplayAlert("Informaci�n", $"Formulario '{formularioEncontrado.TituloFormulario}' deshabilitado.", "OK");

                // Recargar la lista de formularios
                LoadFormularios();
            }
            else
            {
                await DisplayAlert("Error", "Formulario no encontrado.", "OK");
            }
        }
    }
}