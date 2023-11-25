namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class ColumnData
    {
        public ColumnData(string title, int columnNumber, int rowsWithData, int rowsTotal, string candidates)
        {
            Title = title;
            ColumnNumber = columnNumber;
            VoteCount = $"{rowsWithData} votes of {rowsTotal} total";
            Candidates = $"Candidates: {candidates}";
            IsActive = true;
        }

        public string Title { get; set; }
        public int ColumnNumber { get; set; }
        public string VoteCount { get; set; }
        public string Candidates { get; set; }
        public bool IsActive { get; set; }
    }
}
