using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;

namespace AppSalval.ViewModels
{
    public class RiskFormViewModel : INotifyPropertyChanged
    {
        private readonly FactorService _factorService; // Controlador para manejar factores

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id)); // Notificar cambio de propiedad
                }
            }
        }

        private string _textoFactor;
        public string TextoFactor
        {
            get => _textoFactor;
            set
            {
                if (_textoFactor != value)
                {
                    _textoFactor = value ?? string.Empty;
                    OnPropertyChanged(nameof(TextoFactor)); // Notificar cambio de propiedad
                }
            }
        }

        private bool _habilitado;
        public bool Habilitado
        {
            get => _habilitado;
            set
            {
                if (_habilitado != value)
                {
                    _habilitado = value;
                    OnPropertyChanged(nameof(Habilitado)); // Notificar cambio de propiedad
                }
            }
        }

        public ICommand AceptarCommand { get; } // Comando para aceptar

        public RiskFormViewModel()
        {
            _factorService = new FactorService(); // Usa el servicio de factores
            AceptarCommand = new Command(async () => await AgregarFactorAsync()); // Inicializa el comando
        }

        private async Task AgregarFactorAsync()
        {
            try
            {
                var nuevoFactor = new FactorRiesgo
                {
                    TextoFactor = TextoFactor, // Asegurar que el binding es correcto
                    Habilitado = Habilitado
                };

                Console.WriteLine($"Intentando agregar: {nuevoFactor.TextoFactor}, Habilitado: {nuevoFactor.Habilitado}");

                bool success = await _factorService.AddFactorAsync(nuevoFactor); // Llama al servicio para agregar el factor

                Console.WriteLine($"Resultado: {success}");

                await Application.Current.MainPage.DisplayAlert(
                    success ? "Éxito" : "Error",
                    success ? "Factor de riesgo agregado correctamente" : "No se pudo agregar el factor",
                    "OK"
                );

                if (success)
                {
                    // Usando Shell para regresar a la página anterior
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarFactorAsync: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo agregar el factor: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; // Evento para notificar cambios de propiedad
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Invocar el evento
        }
    }
}
