using RankedChoiceVotingTabulator.Wpf.Commands.TieBreaker;
using System.Windows.Input;

namespace RankedChoiceVotingTabulator.Wpf.ViewModels
{
    public class TieBreakerViewModel : ViewModelBase
    {
        public TieBreakerViewModel(NavigationStore navigationStore, ViewModelStore viewModelStore, TieBreakStore tieBreakStore)
        {
            Candidates = new ObservableCollection<Candidate>(tieBreakStore.Candidates);
            TopText = $"There was a tie between the below candidates in round {viewModelStore.HomeViewModel?.ColumnData.Last().Rounds.Last().Number} for {viewModelStore.HomeViewModel?.ColumnData.Last().Title}. Please choose a candidate to eliminate. Exiting will eliminate all of them at once.";

            ViewModelStore = viewModelStore;

            SelectCandidateCommand = new SelectCandidateCommand(this);
            NavigateToHomeCommand = new NavigateToHomeCommand(new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore, viewModelStore)));
        }

        public ObservableCollection<Candidate> Candidates { get; set; }
        public ViewModelStore ViewModelStore { get; set; }
        public string TopText { get; set; }

        public ICommand SelectCandidateCommand { get; }
        public ICommand NavigateToHomeCommand { get; }
    }
}
