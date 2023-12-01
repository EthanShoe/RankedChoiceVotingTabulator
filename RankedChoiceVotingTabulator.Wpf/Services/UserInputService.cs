using OfficeOpenXml;

namespace RankedChoiceVotingTabulator.Wpf.Services
{
    public class UserInputService : IUserInputService
    {
        public Candidate? DoManualTieBreaker(HomeViewModel viewModel, IEnumerable<Candidate> candidatesToBeEliminated)
        {
            var waitHandle = new AutoResetEvent(false);
            CandidateSelectedEventArgs eventData = null;
            EventHandler<CandidateSelectedEventArgs> handler = (sender, args) =>
            {
                eventData = args;
                waitHandle.Set();
            };

            viewModel.CandidateSelected += handler;

            viewModel.NavigateToTieBreakerCommand.Execute(candidatesToBeEliminated.ToList());

            waitHandle.WaitOne();
            var selectedCandidate = eventData?.SelectedCandidate;
            viewModel.CandidateSelected -= handler;
            return selectedCandidate;
        }
    }

    public interface IUserInputService
    {
        Candidate? DoManualTieBreaker(HomeViewModel viewModel, IEnumerable<Candidate> candidatesToBeEliminated);
    }
}
