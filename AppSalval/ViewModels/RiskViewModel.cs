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
        private List<FactorRiesgo> _todosFactores;
        private string _searchText;
        private bool _ordenAscendente = true;
        private string _ordenActual = "Nombre"; // Valor por defecto

        public ObservableCollection<FactorRiesgo> Factores
        {
            get => _factores;
            set
            {
                _factores = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                Buscar(); // Llamar la búsqueda en tiempo real
            }
        }

        public string OrdenActual
        {
            get => _ordenActual;
            set
            {
                _ordenActual = value;
                OnPropertyChanged();
                Ordenar();
            }
        }

        public ICommand NavigateToRiskFormCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand BuscarCommand { get; }
        public ICommand OrdenarCommand { get; }

        public RiskViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _factorService = new FactorService();
            _factores = new ObservableCollection<FactorRiesgo>();
            _todosFactores = new List<FactorRiesgo>();

            NavigateToRiskFormCommand = new Command(async () => await _navigation.PushAsync(new Views.RiskFormPage()));
            DeleteCommand = new Command<FactorRiesgo>(async (factor) => await DeleteFactorAsync(factor));
            EditCommand = new Command<FactorRiesgo>(async (factor) => await EditFactorAsync(factor));

            BuscarCommand = new Command(Buscar);
            OrdenarCommand = new Command(Ordenar);

            LoadFactores();
        }

        private async void LoadFactores()
        {
            var lista = await _factorService.GetFactoresAsync();
            _todosFactores = new List<FactorRiesgo>(lista);
            Factores = new ObservableCollection<FactorRiesgo>(lista);
        }

        private void Buscar()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Factores = new ObservableCollection<FactorRiesgo>(_todosFactores);
            }
            else
            {
                var resultado = _todosFactores
                    .Where(f => f.TextoFactor.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Factores = new ObservableCollection<FactorRiesgo>(resultado);
            }
        }

        private void Ordenar()
        {
            if (OrdenActual == "Nombre")
            {
                Factores = _ordenAscendente
                    ? new ObservableCollection<FactorRiesgo>(Factores.OrderBy(f => f.TextoFactor))
                    : new ObservableCollection<FactorRiesgo>(Factores.OrderByDescending(f => f.TextoFactor));
            }
            else if (OrdenActual == "ID")
            {
                Factores = _ordenAscendente
                    ? new ObservableCollection<FactorRiesgo>(Factores.OrderBy(f => f.IdFactor))
                    : new ObservableCollection<FactorRiesgo>(Factores.OrderByDescending(f => f.IdFactor));
            }

            _ordenAscendente = !_ordenAscendente;
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
                _todosFactores.Remove(factor);
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

