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
        private String _cedula;
        private DateTime _fechaNacimiento;
        private double _altura;
        private double _peso;
        private string _generoSeleccionado;
        public ICommand RegistrarComando { get; }
        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        public String Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }

        public string GeneroSeleccionado
        {
            get => _generoSeleccionado;
            set => SetProperty(ref _generoSeleccionado, value);
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
        }
        public double Altura
        {
            get => _altura;
            set => SetProperty(ref _altura, value);
        }
        public double Peso
        {
            get => _peso;
            set => SetProperty(ref _peso, value);
        }



        public InfoPersonalViewModel()
        {
            RegistrarComando = new Command(async () =>
            {
                try
                {
                    int id = int.Parse(Cedula); ;

                    if (string.IsNullOrWhiteSpace(Cedula))
                    {
                         
                        await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese su cédula.", "OK");
                        return;
                    
                    }else if (Cedula.Length<9)
                    {
                        await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese todos los digitos de su cedula. Minimo deben ser 9 digitos", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Ingrese el formato correcto, solo debe de ingresar numeros en el espacio de cedula", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese su nombre.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(GeneroSeleccionado))
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, seleccione su género.", "OK");
                    return;
                }

                if (FechaNacimiento == default)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese su fecha de nacimiento.", "OK");
                    return;
                }

                if (Altura <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese una altura válida.", "OK");
                    return;
                }

                if (Peso <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, ingrese un peso válido.", "OK");
                    return;
                }

            });
        }
    }

}
