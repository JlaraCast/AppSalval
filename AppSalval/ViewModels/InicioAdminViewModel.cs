using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AppSalval.Views;
using Microsoft.Maui.Controls;

namespace AppSalval.ViewModels
{
    public class InicioAdminViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        public ICommand PestañaFormularios { get; }
        public ICommand PestañaGestionUsuario{ get; }

        public ICommand PestañaGestionResultados { get; }

        // Inyectamos la navegación en el constructor
        public InicioAdminViewModel(INavigation navigation)
        {
            _navigation = navigation;

//----------------------------------------------------------------------------------------------------------

            PestañaFormularios = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionFormularios());
            });
 
//----------------------------------------------------------------------------------------------------------

            PestañaGestionUsuario = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionUsuarios());
            });

//----------------------------------------------------------------------------------------------------------

            PestañaGestionResultados = new Command(async () =>
            {
                await _navigation.PushAsync(new GestionRespuesta());
            });

//----------------------------------------------------------------------------------------------------------



        }



    }
}
