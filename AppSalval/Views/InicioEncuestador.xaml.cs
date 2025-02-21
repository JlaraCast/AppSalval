using Microsoft.Maui.Controls;
using System;

namespace AppSalval.Views
{
    public partial class InicioEncuestador : ContentPage
    {
        public InicioEncuestador()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navega a la página de login
            await Navigation.PushAsync(new LoginPage());
        }

        private async void OnFormularioClicked(object sender, EventArgs e)
        {
            // Navega a la página de encuestas
            await Navigation.PushAsync(new Formularios());
        }
    }
}
