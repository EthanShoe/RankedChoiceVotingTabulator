namespace RankedChoiceVotingTabulator.Wpf.Models
{
    public class Vote
    {
        public Vote(Dictionary<int, Candidate> orderPreference)
        {
            OrderPreference = orderPreference;
            CalculateTopCandidate();
        }

        public Candidate TopCandidate { get; set; }
        public Dictionary<int, Candidate> OrderPreference { get; set; }

        public void CalculateTopCandidate()
        {
            TopCandidate = OrderPreference.Where(x => x.Value.IsActive).OrderBy(x => x.Key).FirstOrDefault().Value;
        }
    }
}
