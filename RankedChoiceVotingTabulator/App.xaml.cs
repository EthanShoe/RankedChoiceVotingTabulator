using OfficeOpenXml;
using RankedChoiceVotingTabulator.Wpf.Stores;
using RankedChoiceVotingTabulator.Wpf.ViewModels;
namespace RankedChoiceVotingTabulator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            NavigationStore navigationStore = new();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore)
            };
            MainWindow.Show();

            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

            base.OnStartup(e);
        }
    }
}
