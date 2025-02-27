
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;
using AppSalval.Views;

namespace AppSalval.ViewModels
{
    public class RiskEditViewModel : BindableObject
    {
        // Servicio para manejar operaciones relacionadas con factores de riesgo
        private readonly FactorService _riskService;
        // Factor de riesgo que se está editando
        private FactorRiesgo _riskFactor;

        // Propiedad para el ID del factor de riesgo
        public string IdFactor
        {
            get => _riskFactor.IdFactor.ToString();
            set
            {
                if (int.TryParse(value, out int id))
                {
                    _riskFactor.IdFactor = id;
                    OnPropertyChanged();
                }
            }
        }

        // Propiedad para el texto del factor de riesgo
        public string TextoFactor
        {
            get => _riskFactor.TextoFactor;
            set
            {
                _riskFactor.TextoFactor = value;
                OnPropertyChanged();
            }
        }

        // Propiedad para el estado del factor de riesgo (Activo/Inactivo)
        public string Estado
        {
            get => _riskFactor.Habilitado ? "Activo" : "Inactivo";
            set
            {
                _riskFactor.Habilitado = value == "Activo";
                OnPropertyChanged();
            }
        }

        // Comando para guardar los cambios del factor de riesgo
        public ICommand GuardarCommand { get; }

        // Constructor que inicializa el servicio y el factor de riesgo
        public RiskEditViewModel(FactorRiesgo riskFactor)
        {
            _riskService = new FactorService();
            _riskFactor = riskFactor;

            // Inicializa el comando de guardar
            GuardarCommand = new Command(async () => await GuardarEdicionAsync());
        }

        // Método para guardar los cambios del factor de riesgo
        private async Task GuardarEdicionAsync()
        {
            bool actualizado = await _riskService.UpdateFactorAsync(_riskFactor);

            if (actualizado)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Tipo de riesgo actualizado", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new RiskPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar", "OK");
            }
        }
    }
}
