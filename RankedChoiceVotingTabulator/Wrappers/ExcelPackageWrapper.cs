using OfficeOpenXml;
using System.IO.Packaging;

namespace RankedChoiceVotingTabulator.Wpf.Wrappers
{
    public class ExcelPackageWrapper : IExcelPackageWrapper
    {
        private ExcelPackage _package;

        public ExcelPackageWrapper(ExcelPackage package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
        }

        public ExcelWorksheet GetFirstSheet()
        {
            return _package.Workbook.Worksheets.FirstOrDefault();
        }
    }

    public interface IExcelPackageWrapper
    {
        ExcelWorksheet GetFirstSheet();
    }
}
