namespace RankedChoiceVotingTabulator.Wpf.Commands
{
    public class NavigateToHomeCommand : CommandBase
    {
        private readonly NavigationService<HomeViewModel> _navigationService;

        public NavigateToHomeCommand(NavigationService<HomeViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate(parameter as HomeViewModel);
        }
    }
}
