using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RankedChoiceVotingTabulator.Wpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(NavigationStore navigationStore)
        {
            ControlsEnabled = true;
            ColumnData = new();

            InputProcessingCommand = new InputProcessingCommand(this);
            TabulateCommand = new TabulateCommand(this);
        }

        private string? _excelFilePath;
        private ObservableCollection<ColumnData> _columnData;
        private ExcelPackageWrapper _excelPackage;
        private bool _controlsEnabled;

        public string? ExcelFilePath { get => _excelFilePath; set { SetProperty(ref _excelFilePath, value); InputProcessingCommand.Execute(null); } }
        public ObservableCollection<ColumnData> ColumnData { get => _columnData; set => SetProperty(ref _columnData, value); }
        public ExcelPackageWrapper ExcelPackage { get => _excelPackage; set => SetProperty(ref _excelPackage, value); }
        public bool ControlsEnabled { get => _controlsEnabled; set { SetProperty(ref _controlsEnabled, value); } }

        public ICommand InputProcessingCommand { get; }
        public ICommand TabulateCommand { get; }
    }
}
