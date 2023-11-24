using RankedChoiceVotingTabulator.Wpf.Stores;
using RankedChoiceVotingTabulator.Wpf.ViewModels;
using RankedChoiceVotingTabulator.Wpf;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RankedChoiceVotingTabulator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

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
