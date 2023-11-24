using OfficeOpenXml;

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
    }

    public interface IExcelWorksheetWrapper
    {
        void SetCell(int row, int column, object value);
        object GetCell(int row, int column);
        int GetRowCount();
        int GetColumnCount();
    }
}
