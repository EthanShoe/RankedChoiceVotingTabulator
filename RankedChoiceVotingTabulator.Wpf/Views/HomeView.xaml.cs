using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RankedChoiceVotingTabulator.Wpf.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void InputFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel Files (.xlsx)|*.xlsx"
            };

            openFileDialog.ShowDialog();

            ((HomeViewModel)DataContext).ExcelFilePath = openFileDialog.FileName;
        }

        private void InputFiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
                if (System.IO.Path.GetExtension(filePath) != ".xlsx")
                {
                    MessageBox.Show("Please insert an Excel file (.xlsx)", "Wrong File Type", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ((HomeViewModel)DataContext).ExcelFilePath = filePath;
            }
        }
    }
}
