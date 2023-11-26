namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class Round
    {
        public Round(int number)
        {
            Number = number;
            CandidateVotes = new Dictionary<Candidate, int>();
        }

        public int Number { get; set; }
        public Dictionary<Candidate, int> CandidateVotes { get; set; }
    }
}
