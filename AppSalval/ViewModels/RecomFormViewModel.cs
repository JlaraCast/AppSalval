using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.Controllers;
using AppSalval.DTOS_API;
using AppSalval.Views;

namespace AppSalval.ViewModels
{
    public class RecomFormViewModel : INotifyPropertyChanged
    {
        private readonly GestionRecomController _recomController;

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    OnPropertyChanged(nameof(Descripcion));
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
                    OnPropertyChanged(nameof(Habilitado));
                }
            }
        }

        public ICommand AceptarCommand { get; }

        public RecomFormViewModel()
        {
            _recomController = new GestionRecomController();
            AceptarCommand = new Command(async () => await AgregarRecomendacionAsync());
        }

        private async Task AgregarRecomendacionAsync()
        {
            var nuevaRecomendacion = new Recomendacion
            {
                TextoRecomendacion = Descripcion,
                Habilitado = Habilitado
            };

            bool success = await _recomController.AddRecomendacionAsync(nuevaRecomendacion);

            await Application.Current.MainPage.DisplayAlert(
                success ? "Éxito" : "Error",
                success ? "Recomendación agregada correctamente" : "No se pudo agregar la recomendación",
                "OK"
            );

            if (success)
            {
                // Usando Shell para regresar a la página anterior
                await Shell.Current.GoToAsync("..");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
