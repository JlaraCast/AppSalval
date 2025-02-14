using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppSalval.ViewModels
{
    public class InfoPersonalViewModel : BaseViewModel
    {
        private string _nombre;
        private String _etiqueta;
        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        public string Etiqueta
        {
            get => _etiqueta;
            set => SetProperty(ref _etiqueta, value);
        }


        private string _apellido;
        public string Apellido
        {
            get => _apellido;
            set => SetProperty(ref _apellido, value);
        }

        private DateTime _fechaNacimiento;
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
        }

        public ICommand RegistrarComando { get; }
        
        
        public InfoPersonalViewModel()
        {
            Etiqueta = Nombre;

            RegistrarComando = new Command(() => {

                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    Etiqueta = "Por favor, ingrese su nombre.";
                    return;
                }


            });

        }



    }
}
