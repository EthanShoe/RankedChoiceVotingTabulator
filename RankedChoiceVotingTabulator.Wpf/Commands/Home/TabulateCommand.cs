namespace RankedChoiceVotingTabulator.Wpf.Commands.Home
{
    public class TabulateCommand : CommandBase
    {
        private HomeViewModel _viewModel;
        private ITabulationService _tabulationService;

        public TabulateCommand(HomeViewModel viewModel, ITabulationService tabulationService)
        {
            _viewModel = viewModel;
            _tabulationService = tabulationService;
        }

        public override void Execute(object parameter)
        {
            _ = MainWork();
        }

        private async Task MainWork()
        {
            AggregateException aggregateException = null;
            if (_viewModel.ExcelFilePath == "") return;

            try
            {
                _viewModel.ControlsEnabled = false;

                var task = Task.Run(() => LongRunningTask());
                await task;
                aggregateException = task.Exception;

                if (aggregateException?.InnerExceptions.Count > 0)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        ShowError(innerException.Message);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            _viewModel.ControlsEnabled = true;
        }

        private void LongRunningTask()
        {
            var tabulationService = new TabulationService(_tabulationService);
            foreach (var columnData in _viewModel.ColumnData.Where(x => x.IsActive))
            {
                if (columnData.Rounds.Count != 0)
                {
                    columnData.Rounds.Clear();
                    columnData.Candidates.ForEach(x => x.Reset());
                    columnData.Votes.ForEach(x => x.CalculateTopCandidate());
                }
                tabulationService.Tabulate(_viewModel, columnData);
                tabulationService.WriteResults(columnData, new ExcelWorksheetWrapper(_viewModel.ExcelPackage.NewSheet(columnData.Title)));
            }

            var response = _viewModel.ExcelPackage.Save();
            while (response == SaveStatus.Failure)
            {
                response = _viewModel.ExcelPackage.Save();
            }
            if (response == SaveStatus.Success)
                _viewModel.ExcelPackage.OpenFile();
        }

        private void ShowError(string message)
        {
            MessageBox.Show("Error occured when tabulating:\n" + message, "Error Opening File", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
