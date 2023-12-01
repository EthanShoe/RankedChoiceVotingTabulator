using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;

namespace RankedChoiceVotingTabulator.Wpf.Services
{
    public class InputProcessingService
    {
        public List<ColumnData> GetColumnData(IExcelWorksheetWrapper excelWorksheet)
        {
            const int FIRST_COLUMN = 6; // Column F is the first column with results

            var result = new List<ColumnData>();
            for (int columnNumber = FIRST_COLUMN; columnNumber <= excelWorksheet.GetColumnCount(); columnNumber++)
            {
                var columnCells = excelWorksheet.GetColumnCellsByColumnNumber(columnNumber);
                var columnCellsWithoutTitle = columnCells.Skip(1).Where(x => !string.IsNullOrEmpty(x));
                if (!columnCellsWithoutTitle.Any() || !columnCellsWithoutTitle.All(x => x.EndsWith(';')))
                    continue;

                result.Add(new ColumnData(
                    columnCells.First(),
                    columnNumber,
                    columnCellsWithoutTitle.ToList(),
                    excelWorksheet.GetRowCount() - 1,
                    columnCellsWithoutTitle.Select(s => s.Split(';').SkipLast(1)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList()
                    ));
            }

            return result;
        }
    }
}
