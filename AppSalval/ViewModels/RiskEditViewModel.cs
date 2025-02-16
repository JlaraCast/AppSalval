
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;

namespace AppSalval.ViewModels
{
    public class RiskEditViewModel : BindableObject
    {
        private readonly FactorService _riskService;
        private FactorRiesgo _riskFactor;

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

        public string TextoFactor
        {
            get => _riskFactor.TextoFactor;
            set
            {
                _riskFactor.TextoFactor = value;
                OnPropertyChanged();
            }
        }

        public string Estado
        {
            get => _riskFactor.Habilitado ? "Activo" : "Inactivo";
            set
            {
                _riskFactor.Habilitado = value == "Activo";
                OnPropertyChanged();
            }
        }

        public ICommand GuardarCommand { get; }


        public RiskEditViewModel(FactorRiesgo riskFactor)
        {
            _riskService = new FactorService();
            _riskFactor = riskFactor;

            GuardarCommand = new Command(async () => await GuardarEdicionAsync());

        }

        private async Task GuardarEdicionAsync()
        {
            bool actualizado = await _riskService.UpdateFactorAsync(_riskFactor);

            if (actualizado)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Tipo de riesgo actualizado", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar", "OK");
            }
        }
    }
}
