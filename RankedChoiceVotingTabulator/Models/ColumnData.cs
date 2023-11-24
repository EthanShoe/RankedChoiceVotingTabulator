namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class ColumnData
    {
        public ColumnData(string name, int columnNumber, int rowsWithData, int rowsTotal)
        {
            Name = name;
            ColumnNumber = columnNumber;
            Info = $"{rowsWithData} entries of {rowsTotal} total rows";
            IsActive = true;
        }

        public string Name { get; set; }
        public int ColumnNumber { get; set; }
        public string Info { get; set; }
        public bool IsActive { get; set; }
    }
}
