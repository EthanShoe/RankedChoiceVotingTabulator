namespace RankedChoiceVotingTabulator.Wpf.Commands
{
    public class NavigateToTieBreakerCommand : CommandBase
    {
        private readonly NavigationService<TieBreakerViewModel> _navigationService;
        private readonly TieBreakStore _tieBreakerStore;

        public NavigateToTieBreakerCommand(NavigationService<TieBreakerViewModel> navigationService, TieBreakStore tieBreakStore)
        {
            _navigationService = navigationService;
            _tieBreakerStore = tieBreakStore;
        }

        public override void Execute(object parameter)
        {
            _tieBreakerStore.Candidates = parameter as List<Candidate>;
            _navigationService.Navigate();
        }
    }
}
