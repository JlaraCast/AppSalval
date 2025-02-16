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
    public class RecomViewModel : BindableObject
    {
        private readonly INavigation _navigation;
        private readonly RecomService _recomService;
        private ObservableCollection<Recomendacion> _recomendaciones;

        public ObservableCollection<Recomendacion> Recomendaciones
        {
            get => _recomendaciones;
            set
            {
                _recomendaciones = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToRecomFormCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }


        public RecomViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _recomService = new RecomService();
            _recomendaciones = new ObservableCollection<Recomendacion>();


            NavigateToRecomFormCommand = new Command(async () => await _navigation.PushAsync(new Views.RecomFormView()));
            DeleteCommand = new Command<Recomendacion>(async (recom) => await DeleteRecomendacionAsync(recom));
            EditCommand = new Command<Recomendacion>(async (recom) => await EditRecomendacionAsync(recom));

            LoadRecomendaciones();
        }

        private async void LoadRecomendaciones()
        {
            var lista = await _recomService.GetRecomendacionesAsync();
            Recomendaciones = new ObservableCollection<Recomendacion>(lista);
        }

        private async Task DeleteRecomendacionAsync(Recomendacion recomendacion)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar", "¿Deseas eliminar esta recomendación?", "Sí", "No");

            if (!confirm) return;

            bool deleted = await _recomService.DeleteRecomendacionAsync(recomendacion);

            if (deleted)
            {
                Recomendaciones.Remove(recomendacion); // 🔹 Eliminamos de la lista visualmente
                await Application.Current.MainPage.DisplayAlert("Éxito", "Recomendación eliminada", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar", "OK");
            }
        }


        private async Task EditRecomendacionAsync(Recomendacion recomendacion)
        {
            await _navigation.PushAsync(new Views.RecomEditFormPage(recomendacion));
        }
    }
}
