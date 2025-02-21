namespace AppSalval.Views
{
    public partial class InfoPersonal : ContentPage
    {
        private int _idFormulario;
        private string _tituloFormulario;

        public InfoPersonal(int idFormulario, string tituloFormulario)
        {
            InitializeComponent();
            _idFormulario = idFormulario;
            _tituloFormulario = tituloFormulario;
        }

        private async void OnContinuarClicked(object sender, EventArgs e)
        {
            // Navegar a AplicarFormulario.xaml con el formulario correspondiente
            await Navigation.PushAsync(new AplicarFormulario(_idFormulario, _tituloFormulario));
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            // Volver a Formularios.xaml
            await Navigation.PopAsync();
        }
    }
}