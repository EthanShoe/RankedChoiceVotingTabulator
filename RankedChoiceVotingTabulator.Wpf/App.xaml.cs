using OfficeOpenXml;
namespace RankedChoiceVotingTabulator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ITabulationService, TabulationService>();
            services.AddTransient<HomeViewModel>();
            services.AddSingleton<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            NavigationStore navigationStore = new();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.DataContext = new MainWindowViewModel(navigationStore);
            MainWindow.Show();

            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore, new ViewModelStore(), _serviceProvider.GetRequiredService<ITabulationService>());

            base.OnStartup(e);
        }
    }
}
