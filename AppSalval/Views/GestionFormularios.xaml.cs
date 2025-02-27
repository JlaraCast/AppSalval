
using AppSalval.Models_Api;
using AppSalval.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

using AppSalval.ViewModels;
namespace AppSalval.Views;

// Definición de la clase parcial GestionFormularios que hereda de ContentPage
public partial class GestionFormularios : ContentPage
{
    // Constructor de la clase GestionFormularios
    public GestionFormularios()
    {
        InitializeComponent(); // Inicializa los componentes de la interfaz de usuario
        _apiService = new ApiServiceFormularios(); // Inicializa el servicio de la API
        LoadFormularios(); // Llama al método para cargar los formularios cuando se carga la página

        this.Appearing += OnPageAppearing; // Suscribe el evento Appearing al método OnPageAppearing
    }

    private readonly ApiServiceFormularios _apiService; // Servicio para conectar con la API

    // Método que se ejecuta cuando la página aparece
    private void OnPageAppearing(object sender, EventArgs e)
    {
        LoadFormularios(); // Llama al método para cargar los formularios
    }

    // Método asíncrono para cargar los formularios
    private async void LoadFormularios()
    {
        // Crea una instancia del ViewModel y le pasa la navegación y la lista de formularios
        GestionFormularioViewModel ViewModel = new GestionFormularioViewModel(Navigation, ListaFormularios);
        ViewModel.LoadFormularios(); // Llama al método para cargar los formularios en el ViewModel
        BindingContext = ViewModel; // Establece el contexto de enlace de datos para la página
    }
}



// Definición de la clase FormularioViewModel
public class FormularioViewModel
{
    public int Id { get; set; } // Propiedad para el ID del formulario
    public string NombreDescripcion { get; set; } // Propiedad para la descripción del formulario
    public Command VerCommand { get; set; } // Comando para ver el formulario
    public Command EditarCommand { get; set; } // Comando para editar el formulario
    public Command EliminarCommand { get; set; } // Comando para eliminar el formulario
}
