namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class ColumnData
    {
        public ColumnData(string title, int columnNumber, int rowsWithData, int rowsTotal, List<string> candidates)
        {
            Title = title;
            ColumnNumber = columnNumber;
            VoteCount = $"{rowsWithData} votes of {rowsTotal} total";
            IsActive = true;
            Candidates = candidates;
        }

        public string Title { get; set; }
        public int ColumnNumber { get; set; }
        public string VoteCount { get; set; }
        public List<string> Candidates { get; set; }
        public string CandidatesString { get { return $"Candidates: {string.Join(", ", Candidates)}"; } }
        public bool IsActive { get; set; }
    }
}
