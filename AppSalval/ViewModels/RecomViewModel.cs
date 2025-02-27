using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.DTOS_API;
using AppSalval.Services;
using AppSalval.Views;

namespace AppSalval.ViewModels
{
    public class RecomViewModel : BindableObject
    {
        // Campos privados
        private readonly INavigation _navigation;
        private readonly RecomService _recomService;
        private ObservableCollection<Recomendacion> _recomendaciones;
        private List<Recomendacion> _todasRecomendaciones;
        private string _searchText;
        private bool _ordenAscendente = true;
        private string _ordenActual = "ID"; // Valor por defecto

        // Propiedades públicas
        public ObservableCollection<Recomendacion> Recomendaciones
        {
            get => _recomendaciones;
            set
            {
                _recomendaciones = value;
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
                Buscar();
            }
        }

        public string OrdenActual
        {
            get => _ordenActual;
            set
            {
                _ordenActual = value;
                OnPropertyChanged();
            }
        }

        // Comandos públicos
        public ICommand NavigateToRecomFormCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand OrdenarCommand { get; }

        // Constructor
        public RecomViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _recomService = new RecomService();
            _recomendaciones = new ObservableCollection<Recomendacion>();
            _todasRecomendaciones = new List<Recomendacion>();

            // Inicialización de comandos
            NavigateToRecomFormCommand = new Command(async () => await _navigation.PushAsync(new Views.RecomFormView()));
            DeleteCommand = new Command<Recomendacion>(async (recom) => await DeleteRecomendacionAsync(recom));
            EditCommand = new Command<Recomendacion>(async (recom) => await EditRecomendacionAsync(recom));
            OrdenarCommand = new Command(Ordenar);

            // Cargar recomendaciones
            LoadRecomendaciones();
        }

        // Método para cargar recomendaciones
        private async void LoadRecomendaciones()
        {
            var lista = await _recomService.GetRecomendacionesAsync();
            _todasRecomendaciones = new List<Recomendacion>(lista);
            Recomendaciones = new ObservableCollection<Recomendacion>(lista);
        }

        // Método para buscar recomendaciones
        private void Buscar()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Recomendaciones = new ObservableCollection<Recomendacion>(_todasRecomendaciones);
            }
            else
            {
                var resultado = _todasRecomendaciones
                    .Where(r => r.TextoRecomendacion.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Recomendaciones = new ObservableCollection<Recomendacion>(resultado);
            }
        }

        // Método para ordenar recomendaciones
        private void Ordenar()
        {
            if (OrdenActual == "ID")
            {
                Recomendaciones = _ordenAscendente
                    ? new ObservableCollection<Recomendacion>(Recomendaciones.OrderBy(r => r.IdRecomendacion))
                    : new ObservableCollection<Recomendacion>(Recomendaciones.OrderByDescending(r => r.IdRecomendacion));
            }
            else
            {
                Recomendaciones = _ordenAscendente
                    ? new ObservableCollection<Recomendacion>(Recomendaciones.OrderBy(r => r.TextoRecomendacion))
                    : new ObservableCollection<Recomendacion>(Recomendaciones.OrderByDescending(r => r.TextoRecomendacion));
            }

            _ordenAscendente = !_ordenAscendente;
        }

        // Método para eliminar una recomendación
        private async Task DeleteRecomendacionAsync(Recomendacion recomendacion)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar", "¿Deseas eliminar esta recomendación?", "Sí", "No");

            if (!confirm) return;

            bool deleted = await _recomService.DeleteRecomendacionAsync(recomendacion);

            if (deleted)
            {
                Recomendaciones.Remove(recomendacion);
                _todasRecomendaciones.Remove(recomendacion);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Recomendación eliminada", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new RecomPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
            }
        }

        // Método para editar una recomendación
        private async Task EditRecomendacionAsync(Recomendacion recomendacion)
        {
            await _navigation.PushAsync(new Views.RecomEditFormPage(recomendacion));
        }
    }
}


