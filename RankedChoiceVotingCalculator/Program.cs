using OfficeOpenXml;
using RankedChoiceVotingCalculator.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RankedChoiceVotingCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //open results file
            if (args.Length == 0)
            {
                Console.WriteLine("Please input an Excel file by dragging it over the exe");
                Console.ReadLine();
                return;
            }
            string resultsFilePath = args[0];

            if (Path.GetExtension(resultsFilePath) != ".xlsx")
            {
                Console.WriteLine("Please only input an Excel file (.xlsx)");
                Console.ReadLine();
                return;
            }

            FileInfo fileInfo = new FileInfo(resultsFilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet resultsWorksheet = package.Workbook.Worksheets.FirstOrDefault();
            int numOfRows = resultsWorksheet.Dimension.Rows;
            int numOfColumns = resultsWorksheet.Dimension.Columns;

            //get starting column and row
            int firstColumn = ExcelColumnNameToNumber("F");
            int firstRow = 2;

            //for each column, calculate results
            for (int columnLoop = firstColumn; columnLoop <= numOfColumns; columnLoop++)
            {
                VoteCategory voteCategory = new VoteCategory(resultsWorksheet.Cells[1, columnLoop].Value.ToString(), resultsWorksheet.Cells[firstRow, columnLoop].Value.ToString(), numOfRows - firstRow + 1);
                for (int rowLoop = firstRow; rowLoop <= numOfRows; rowLoop++)
                {
                    voteCategory.AddVotes(resultsWorksheet.Cells[rowLoop, columnLoop].Value.ToString());
                }
                voteCategory.CalculateResults();
                voteCategory.PrintResults(package.Workbook);
            }

            bool fileSuccessfullySaved = false;
            while (!fileSuccessfullySaved)
            {
                try
                {
                    //save file
                    package.Save();
                    fileSuccessfullySaved = true;
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Save failed: please close the Excel file that you submitted and press enter to try again");
                    Console.ReadLine();
                }
            }
            
            //open Excel file
            Process.Start(new ProcessStartInfo(resultsFilePath) { UseShellExecute = true });
        }

        private static int ExcelColumnNameToNumber(string columnTitle)
        {
            return columnTitle.Select((c, i) => ((c - 'A' + 1) * ((int)Math.Pow(26, columnTitle.Length - i - 1)))).Sum();
        }
    }
}
