
using AppSalval.Models_Api;
using AppSalval.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

using AppSalval.ViewModels;
namespace AppSalval.Views;

public partial class GestionFormularios : ContentPage
{
	public GestionFormularios()
	{
		InitializeComponent();
        _apiService = new ApiServiceFormularios();
        LoadFormularios(); // Llamamos a la API cuando se carga la p�gina

    }
    private readonly ApiServiceFormularios _apiService; // Servicio para conectar con la API


    private async void LoadFormularios()
    {
        GestionFormularioViewModel ViewModel = new GestionFormularioViewModel(Navigation, ListaFormularios);
        ViewModel.LoadFormularios(); 
        BindingContext = ViewModel;
       
    }
}


    public class FormularioViewModel
    {
        public int Id { get; set; }
        public string NombreDescripcion { get; set; }
        public Command VerCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command EliminarCommand { get; set; }
    }
