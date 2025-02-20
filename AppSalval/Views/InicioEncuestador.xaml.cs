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

        private async void OnFormularioClicked(object sender, EventArgs e)
        {
            // ?? Aquí puedes redirigir a la vista donde se muestran los formularios
            await Navigation.PushAsync(new Formularios());
        }
    }
}
