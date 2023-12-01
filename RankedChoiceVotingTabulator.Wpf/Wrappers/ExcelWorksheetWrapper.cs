using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace RankedChoiceVotingTabulator.Wpf.Wrappers
{
    public class ExcelWorksheetWrapper : IExcelWorksheetWrapper
    {
        private ExcelWorksheet _worksheet;

        public ExcelWorksheetWrapper(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
        }

        public void SetCell(int row, int column, object value)
        {
            _worksheet.Cells[row, column].Value = value;
        }

        public void SetCellsRow<T>(int startingRow, int startingColumn, List<T> values)
        {
            for (int loop = 0; loop < values.Count; loop++)
            {
                SetCell(startingRow, startingColumn + loop, values[loop]);
            }
        }

        public object GetCell(int row, int column)
        {
            return _worksheet.Cells[row, column].Value;
        }

        public int GetRowCount()
        {
            return _worksheet.Dimension.Rows;
        }

        public int GetColumnCount()
        {
            return _worksheet.Dimension.Columns;
        }

        public List<string> GetColumnCellsByColumnNumber(int columnNumber)
        {
            var columnCells = _worksheet.Cells[1, columnNumber, GetRowCount(), columnNumber];
            return columnCells.Select(cell => cell.Value?.ToString()).ToList();
        }

        public void SetAllColumnsAutoWidth()
        {
            _worksheet.Cells[_worksheet.Dimension.Address].AutoFitColumns();
        }

        public void ColorCell(int rowNumber, int columnNumber, Color color)
        {
            _worksheet.Cells[rowNumber, columnNumber].Style.Fill.PatternType = ExcelFillStyle.Solid;
            _worksheet.Cells[rowNumber, columnNumber].Style.Fill.BackgroundColor.SetColor(color);
        }
    }

    public interface IExcelWorksheetWrapper
    {
        void SetCell(int row, int column, object value);
        void SetCellsRow<T>(int startingRow, int startingColumn, List<T> values);
        object GetCell(int row, int column);
        int GetRowCount();
        int GetColumnCount();
        List<string> GetColumnCellsByColumnNumber(int columnNumber);
        void SetAllColumnsAutoWidth();
        void ColorCell(int rowNumber, int columnNumber, Color color);
    }
}
