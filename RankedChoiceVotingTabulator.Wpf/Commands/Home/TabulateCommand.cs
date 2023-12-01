using OfficeOpenXml;
using System.Diagnostics;
using System.IO;

namespace RankedChoiceVotingTabulator.Wpf.Commands.Home
{
    public class TabulateCommand : CommandBase
    {
        private HomeViewModel _viewModel;

        public TabulateCommand(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
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
            var tabulationService = new TabulationService();
            foreach (var columnData in _viewModel.ColumnData.Where(x => x.IsActive))
            {
                if (columnData.Rounds.Count != 0)
                {
                    columnData.Rounds.Clear();
                    columnData.Candidates.ForEach(x => x.Reset());
                    columnData.Votes.ForEach(x => x.CalculateTopCandidate());
                }
                new TabulationService().Tabulate(_viewModel, columnData, new UserInputService());
                TabulationService.WriteResults(columnData, new ExcelWorksheetWrapper(_viewModel.ExcelPackage.NewSheet(columnData.Title)));
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
