using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;

namespace AppSalval.ViewModels
{
    public class RiskViewModel : BindableObject
    {
        private readonly INavigation _navigation;
        private readonly FactorService _factorService;
        private ObservableCollection<FactorRiesgo> _factores;

        public ObservableCollection<FactorRiesgo> Factores
        {
            get => _factores;
            set
            {
                _factores = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToRiskFormCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public RiskViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _factorService = new FactorService();
            _factores = new ObservableCollection<FactorRiesgo>();

            NavigateToRiskFormCommand = new Command(async () => await _navigation.PushAsync(new Views.RiskFormPage()));
            DeleteCommand = new Command<FactorRiesgo>(async (factor) => await DeleteFactorAsync(factor));
            EditCommand = new Command<FactorRiesgo>(async (factor) => await EditFactorAsync(factor));

            LoadFactores();
        }
        private async void LoadFactores()
        {
            var lista = await _factorService.GetFactoresAsync();
            Factores = new ObservableCollection<FactorRiesgo>(lista);
        }

        private async Task DeleteFactorAsync(FactorRiesgo factor)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar", "¿Deseas eliminar este factor de riesgo?", "Sí", "No");

            if (!confirm) return;

            bool deleted = await _factorService.DeleteFactorAsync(factor);

            if (deleted)
            {
                Factores.Remove(factor);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Factor eliminado", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
            }
        }

        private async Task EditFactorAsync(FactorRiesgo factor)
        {
            await _navigation.PushAsync(new Views.RiskEditFormPage(factor));
        }
    }
}
