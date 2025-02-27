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
    public class RecomEditViewModel : BindableObject
    {
        private readonly RecomService _recomService;
        private Recomendacion _recomendacion;

        public string Descripcion
        {
            get => _recomendacion.TextoRecomendacion;
            set
            {
                _recomendacion.TextoRecomendacion = value;
                OnPropertyChanged();
            }
        }

        public string Estado
        {
            get => _recomendacion.Habilitado ? "Activo" : "Inactivo";
            set
            {
                _recomendacion.Habilitado = value == "Activo"; // 🔹 Convertimos de string a bool
                OnPropertyChanged();
            }
        }


        public ICommand AceptarCommand { get; }


        public RecomEditViewModel(Recomendacion recomendacion)
        {
            _recomService = new RecomService();
            _recomendacion = recomendacion;

            AceptarCommand = new Command(async () => await GuardarEdicionAsync());

        }

        private async Task GuardarEdicionAsync()
        {
            bool actualizado = await _recomService.UpdateRecomendacionAsync(_recomendacion);

            if (actualizado)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Recomendación actualizada", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new RecomPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar", "OK");
            }
        }
    }
}
