namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class Candidate
    {
        public Candidate(string name)
        {
            Name = name;
            IsActive = true;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
