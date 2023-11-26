namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class ColumnData
    {
        public ColumnData(string title, int columnNumber, List<string> votes, int totalRows, List<string> candidates)
        {
            Title = title;
            ColumnNumber = columnNumber;
            TotalRows = totalRows;
            Rounds = new List<Round>();
            IsActive = true;

            Candidates = new List<Candidate>();
            candidates.ForEach(x => Candidates.Add(new Candidate(x)));

            Votes = new List<Vote>();
            foreach (var vote in votes)
            {
                var voteSplit = vote.Split(';').SkipLast(1);
                var orderPreference = new Dictionary<int, Candidate>();
                for (int loop = 1; loop <= voteSplit.Count(); loop++)
                {
                    orderPreference.Add(loop, Candidates.FirstOrDefault(x => x.Name == voteSplit.ElementAt(loop - 1)));
                }
                Votes.Add(new Vote(orderPreference));
            }
        }

        public string Title { get; set; }
        public int ColumnNumber { get; set; }
        public List<Vote> Votes { get; set; }
        public string VoteCount { get => $"{Votes.Count} votes of {TotalRows} total"; }
        private int TotalRows { get; set; }
        public List<Candidate> Candidates { get; set; }
        public string CandidatesString { get => $"Candidates: {string.Join(", ", Candidates.Select(x => x.Name))}"; }
        public List<Round> Rounds { get; set; }
        public bool IsActive { get; set; }
    }
}
