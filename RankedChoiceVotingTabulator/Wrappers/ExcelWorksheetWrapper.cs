using OfficeOpenXml;

namespace RankedChoiceVotingTabulator.Wpf.Wrappers
{
    public class ExcelWorksheetWrapper : IExcelWorksheetWrapper
    {
        private ExcelWorksheet _worksheet;

        public ExcelWorksheetWrapper(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
            RowCount = _worksheet.Dimension.Rows;
            ColumnCount = _worksheet.Dimension.Columns;
        }

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public void SetCell(int row, int column, object value)
        {
            _worksheet.Cells[row, column].Value = value;
        }

        public object GetCell(int row, int column)
        {
            return _worksheet.Cells[row, column].Value;
        }

        public List<string> GetColumnCellsByColumnNumber(int columnNumber)
        {
            var columnCells = _worksheet.Cells[1, columnNumber, RowCount, columnNumber];
            return columnCells.Select(cell => cell.Value?.ToString()).ToList();
        }
    }

    public interface IExcelWorksheetWrapper
    {
        void SetCell(int row, int column, object value);
        object GetCell(int row, int column);
        List<string> GetColumnCellsByColumnNumber(int columnNumber);
    }
}
