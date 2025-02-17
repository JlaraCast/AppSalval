using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;
using AppSalval.Views;

namespace AppSalval.ViewModels
{
    public partial class CrearFormularioViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private string _titulo;
        private string _descripcion;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _requiereDatosPersonales;
        private bool _habilitado;
        private bool _checkboxPregunta;

        public ICommand BtnCancelar { get; }
        public ICommand BtnAceptar{ get; }

        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set => SetProperty(ref _fechaInicio, value);
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set => SetProperty(ref _fechaFin, value);
        }

        public bool Habilitado
        {
            get => _habilitado;
            set => SetProperty(ref _habilitado, value);
        }

        public bool RequiereDatosPersonales
        {
            get => _requiereDatosPersonales;
            set => SetProperty(ref _requiereDatosPersonales, value);
        }

        public bool CheckboxPregunta
        {
            get => _checkboxPregunta;
            set => SetProperty(ref _checkboxPregunta, value);
        }





        public CrearFormularioViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _titulo = string.Empty;
            _descripcion = string.Empty;
            _fechaInicio = DateTime.Now;
            _fechaFin = DateTime.Now;
            _requiereDatosPersonales = false;
            _habilitado = true;
            _checkboxPregunta = false;


            BtnCancelar = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionFormularios());
            });

            /* ingresar la funcionalidad de la creacion de un formulario
            BtnCancelar = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionFormularios());
            });

            */

        }// Fin del constructor


    }//fin de la clase
}//Fin del Namespace
