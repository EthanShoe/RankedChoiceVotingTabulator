namespace RankedChoiceVotingTabulator.Wpf.Services
{
    public class NavigationService<TViewModel>
       where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(TViewModel? viewModel = null)
        {
            if (viewModel == null)
                _navigationStore.CurrentViewModel = _createViewModel();
            else
                _navigationStore.CurrentViewModel = viewModel;
        }
    }
}
