using AppSalval.ViewModels;

namespace AppSalval.Views;

public partial class GestionParticipantes : ContentPage
{
    public GestionParticipantes()
    {
        InitializeComponent();
        BindingContext = new GestionParticipantesViewModel();
    }
}