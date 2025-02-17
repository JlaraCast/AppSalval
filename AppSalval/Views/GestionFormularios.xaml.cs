
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
<<<<<<< Updated upstream
        GestionFormularioViewModel PestañaViewModel = new GestionFormularioViewModel(Navigation, ListaFormularios);
        PestañaViewModel.LoadFormularios(); // Llamamos a la API cuando se carga la página
        BindingContext = PestañaViewModel;
       // ((GestionFormularioViewModel)BindingContext).LoadFormularios(); // Llamamos al método LoadFormularios del ViewModel
=======
        GestionFormularioViewModel PestanaNueva = new GestionFormularioViewModel(Navigation, ListaFormularios);
        PestanaNueva.LoadFormularios(); // Llamamos a la API cuando se carga la pï¿½gina
        BindingContext = PestanaNueva;
       // ((GestionFormularioViewModel)BindingContext).LoadFormularios(); // Llamamos al mï¿½todo LoadFormularios del ViewModel
>>>>>>> Stashed changes
    }
    
}

