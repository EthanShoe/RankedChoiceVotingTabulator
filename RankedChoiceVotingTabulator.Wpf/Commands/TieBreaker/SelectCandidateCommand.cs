using RankedChoiceVotingTabulator.Wpf.Stores;

namespace RankedChoiceVotingTabulator.Wpf.Commands.TieBreaker
{
    public class SelectCandidateCommand : CommandBase
    {
        private TieBreakerViewModel _viewModel;

        public SelectCandidateCommand(TieBreakerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.ViewModelStore.HomeViewModel.InvokeCandidateSelected(parameter as Candidate);
            _viewModel.NavigateToHomeCommand.Execute(_viewModel.ViewModelStore.HomeViewModel);
        }
    }
}
