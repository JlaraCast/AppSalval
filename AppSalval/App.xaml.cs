namespace AppSalval
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzcyMTEwMkAzMjM4MmUzMDJlMzBhMU1TUFZjNzhsaVp0ZWRrcUhVR1JlbUt6UjlWNEYrNzI4Njg0RlAzTElVPQ==");
            Application.Current.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}