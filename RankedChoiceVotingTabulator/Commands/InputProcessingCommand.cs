using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Windows;

namespace RankedChoiceVotingTabulator.Wpf.Commands
{
    public class InputProcessingCommand : CommandBase
    {
        private HomeViewModel _viewModel;

        public InputProcessingCommand(HomeViewModel viewModel)
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

                var task = Task.Run(() => LongRunningTask(_viewModel.ExcelFilePath));
                var results = await task;
                aggregateException = task.Exception;

                if (aggregateException?.InnerExceptions.Count > 0)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        ShowError(innerException.Message);
                    }
                    return;
                }

                _viewModel.ColumnData = new ObservableCollection<ColumnData>(results);
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
            }

            _viewModel.ControlsEnabled = true;
        }

        private List<ColumnData> LongRunningTask(string excelFilePath)
        {
            var fileInfo = new FileInfo(excelFilePath);
            _viewModel.ExcelPackage = new ExcelPackageWrapper(new ExcelPackage(fileInfo));
            var mainWorksheet = new ExcelWorksheetWrapper(_viewModel.ExcelPackage.GetFirstSheet());
            const int FIRST_COLUMN = 6; // Column F is the first column with results

            var result = new List<ColumnData>();
            for (int columnNumber = FIRST_COLUMN; columnNumber <= mainWorksheet.ColumnCount; columnNumber++)
            {
                var columnCells = mainWorksheet.GetColumnCellsByColumnNumber(columnNumber);
                var columnCellsWithoutTitle = columnCells.Skip(1).Where(x => !string.IsNullOrEmpty(x));
                result.Add(new ColumnData(
                    columnCells.First(),
                    columnNumber,
                    columnCellsWithoutTitle.Count(),
                    mainWorksheet.RowCount - 1, 
                    string.Join(", ", columnCellsWithoutTitle.First().Split(";").SkipLast(1).ToArray())
                    ));
            }
            return result;
        }

        private void ShowError(string message)
        {
            MessageBox.Show("Error occured when opening Excel file:\n" + message, "Error Opening File", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
