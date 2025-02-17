
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
    private readonly ApiServiceFormularios _apiService; // Servicio para conectar con la API

    public GestionFormularios()
    {
        InitializeComponent();
        _apiService = new ApiServiceFormularios();
        GestionFormularioViewModel PestañaViewModel = new GestionFormularioViewModel(Navigation, ListaFormularios);
        PestañaViewModel.LoadFormularios(); // Llamamos a la API cuando se carga la página
        BindingContext = PestañaViewModel;
       // ((GestionFormularioViewModel)BindingContext).LoadFormularios(); // Llamamos al método LoadFormularios del ViewModel
    }
    
}

