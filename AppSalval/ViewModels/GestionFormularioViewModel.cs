using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using AppSalval.Views;
using AppSalval.Models_Api;
using AppSalval.Services;
using System.Security.Cryptography.X509Certificates;

namespace AppSalval.ViewModels
{
    public  class GestionFormularioViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly ApiServiceFormularios _apiService; // Servicio para conectar con la API
        private CollectionView _listaFormularios;
        public ICommand BtnAgregar { get; }
        public ICommand BtnOpciones{ get; }
        public ICommand BtnMenuPrincipal{ get; }
        public GestionFormularioViewModel(INavigation navigation, CollectionView listaFormularios) {
            
            _listaFormularios = listaFormularios;
            _navigation = navigation;
            _apiService = new ApiServiceFormularios();


            BtnAgregar = new Command(async () =>
            {
                await _navigation.PushAsync(new CrearFormulario());
            });

            BtnAgregar = new Command(async () =>
            {
                await _navigation.PushAsync(new CrearFormulario());
            });
        }



        public async void LoadFormularios()
        {
            try
            {
                List<FormularioDto> formularios = await _apiService.GetFormularios();

                if (formularios != null && formularios.Count > 0)
                {
                    // Filtramos los formularios habilitados y agregamos un número a cada uno
                    var formulariosConFormato = formularios
                        .Where(f => f.Habilitado)
                        .Select((f, index) => new FormularioViewModel
                        {
                            Id = f.IdFormulario,
                            Nombre = f.TituloFormulario,
                            Descripcion = f.DescripcionFormulario,
                            VerCommand = new Command(() => OnVerFormularioClicked(f)),
                            EditarCommand = new Command(() => OnEditarFormularioClicked(f)),
                            EliminarCommand = new Command(() => OnEliminarFormularioClicked(f))
                        })
                        .ToList();

                    _listaFormularios.ItemsSource = formulariosConFormato;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "No hay formularios disponibles", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar los formularios: {ex.Message}", "OK");
            }
        }

        private void OnOpcionesClicked(object sender, EventArgs e)
        {
            // Lógica para desplegar opciones
            Console.WriteLine("Opciones clicked.");
        }

        private void OnMenuPrincipalClicked(object sender, EventArgs e)
        {
            // Lógica para salir al menú principal
            Console.WriteLine("Menú Principal clicked.");
        }

        private async void OnVerFormularioClicked(FormularioDto formulario)
        {
            // Lógica para ver el formulario
            await _navigation.PushAsync(new VerFormularioCreado(formulario));
            Debug.WriteLine($"Ver formulario: {formulario.TituloFormulario}");
        }

        private async void OnEditarFormularioClicked(FormularioDto formulario)
        {
            // Lógica para editar el formulario
            await _navigation.PushAsync(new EditarFormulario(formulario));
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

                // Llamar al método para actualizar la base de datos
                await _apiService.EditFormulario(formularioEncontrado);

                // Mostrar mensaje de confirmación
                await Application.Current.MainPage.DisplayAlert("Información", $"Formulario '{formularioEncontrado.TituloFormulario}' deshabilitado.", "OK");

                // Recargar la lista de formularios
                LoadFormularios();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Formulario no encontrado.", "OK");
            }
        }
    

    }//fin de la clase

    public class FormularioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Command VerCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command EliminarCommand { get; set; }
    }

}//Fin del namespace
