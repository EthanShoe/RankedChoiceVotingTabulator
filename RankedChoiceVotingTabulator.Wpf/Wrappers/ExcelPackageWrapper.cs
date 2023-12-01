using OfficeOpenXml;
using System.Diagnostics;
using System.Drawing;
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

        public ExcelWorksheet NewSheet(string title)
        {
            var existingSheet = _package.Workbook.Worksheets.FirstOrDefault(x => x.Name == title);
            if (existingSheet != null)
            {
                _package.Workbook.Worksheets.Delete(existingSheet);
            }
            return _package.Workbook.Worksheets.Add(title);
        }

        public SaveStatus Save()
        {
            try
            {
                _package.Save();
                return SaveStatus.Success;
            }
            catch (InvalidOperationException)
            {
                var response = MessageBox.Show("Save failed: please close the Excel file that you submitted and press OK to try again", "Failed to Save Excel File", MessageBoxButton.OKCancel);
                switch (response)
                {
                    case MessageBoxResult.OK:
                        return SaveStatus.Failure;
                    case MessageBoxResult.Cancel:
                        return SaveStatus.Cancel;
                    default:
                        throw new Exception("Unknown response from MessageBox");
                }
            }
        }

        public void OpenFile()
        {
            Process.Start(new ProcessStartInfo(_package.File.FullName) { UseShellExecute = true });
        }
    }

    public interface IExcelPackageWrapper
    {
        ExcelWorksheet GetFirstSheet();
        ExcelWorksheet NewSheet(string title);
        SaveStatus Save();
        void OpenFile();
    }

    public enum SaveStatus
    {
        Success,
        Failure,
        Cancel
    }
}
