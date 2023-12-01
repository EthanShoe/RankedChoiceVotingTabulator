using RankedChoiceVotingTabulator.Wpf.Commands.Home;
using RankedChoiceVotingTabulator.Wpf.Stores;
using System.Windows.Input;

namespace RankedChoiceVotingTabulator.Wpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel() { }

        public HomeViewModel(NavigationStore navigationStore, ViewModelStore viewModelStore)
        {
            ControlsEnabled = true;
            ColumnData = new();
            ManualTieBreaking = false;

            viewModelStore.HomeViewModel = this;

            var newTieBreakStore = new TieBreakStore();
            NavigateToTieBreakerCommand = new NavigateToTieBreakerCommand(new NavigationService<TieBreakerViewModel>(navigationStore, () => new TieBreakerViewModel(navigationStore, viewModelStore, newTieBreakStore)), newTieBreakStore);
            InputProcessingCommand = new InputProcessingCommand(this);
            TabulateCommand = new TabulateCommand(this);
        }

        private string? _excelFilePath;
        private ObservableCollection<ColumnData> _columnData;
        private ExcelPackageWrapper _excelPackage;
        private bool _manualTieBreaking;
        private bool _controlsEnabled;

        public string? ExcelFilePath { get => _excelFilePath; set { SetProperty(ref _excelFilePath, value); InputProcessingCommand?.Execute(null); } }
        public ObservableCollection<ColumnData> ColumnData { get => _columnData; set => SetProperty(ref _columnData, value); }
        public ExcelPackageWrapper ExcelPackage { get => _excelPackage; set => SetProperty(ref _excelPackage, value); }
        public bool ManualTieBreaking { get => _manualTieBreaking; set { SetProperty(ref _manualTieBreaking, value); } }
        public bool ControlsEnabled { get => _controlsEnabled; set { SetProperty(ref _controlsEnabled, value); } }

        public ICommand NavigateToTieBreakerCommand { get; }
        public ICommand InputProcessingCommand { get; }
        public ICommand TabulateCommand { get; }

        public event EventHandler<CandidateSelectedEventArgs> CandidateSelected;

        public void InvokeCandidateSelected(Candidate selectedCandidate)
        {
            CandidateSelected.Invoke(this, new CandidateSelectedEventArgs(selectedCandidate));
        }
    }

    public class CandidateSelectedEventArgs : EventArgs
    {
        public Candidate SelectedCandidate { get; }

        public CandidateSelectedEventArgs(Candidate selectedCandidate)
        {
            SelectedCandidate = selectedCandidate;
        }
    }
}
