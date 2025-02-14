namespace AppSalval
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzcyMDU0NkAzMjM4MmUzMDJlMzBhMU1TUFZjNzhsaVp0ZWRrcUhVR1JlbUt6UjlWNEYrNzI4Njg0RlAzTElVPQ==");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}