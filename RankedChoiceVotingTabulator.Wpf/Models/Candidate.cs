namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class Candidate
    {
        public Candidate(string name)
        {
            Name = name;
            Status = CandidateStatus.Active;
        }

        public string Name { get; set; }
        public CandidateStatus Status { get; set; }
        public int? RoundEliminated { get; set; }

        public void Reset()
        {
            Status = CandidateStatus.Active;
            RoundEliminated = null;
        }

        public enum CandidateStatus
        {
            Active,
            Eliminated,
            Winner
        }
    }
}
