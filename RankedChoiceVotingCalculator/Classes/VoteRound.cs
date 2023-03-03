using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RankedChoiceVotingCalculator.Classes.Candidate;

namespace RankedChoiceVotingCalculator.Classes
{
    public class VoteRound
    {
        public VoteRound(int roundNumber, List<Candidate> candidates, List<Vote> votes, int minimumThreshold)
        {
            RoundNumber = roundNumber;
            Candidates = new List<Candidate>();
            foreach (Candidate candidate in candidates)
                Candidates.Add(candidate.Clone());
            MinimumThreshold = minimumThreshold;
            Votes = new List<Vote>(votes);
        }

        public int RoundNumber { get; set; }
        public List<Candidate> Candidates { get; set; }
        public List<Vote> Votes { get; set; }
        public int MinimumThreshold { get; set; }

        public WinnerSearchResult SearchForWinner()
        {
            List<Candidate> candidates = new List<Candidate>();
            candidates.AddRange(Candidates);

            //count number of first place votes for each candidate
            foreach (Vote vote in Votes)
            {
                for (int loop = 1; loop <= candidates.Count; loop++)
                {
                    Candidate currentPreference = candidates.First(x => x.Name == vote.OrderPreference[loop]);
                    if (currentPreference.Status != CandidateStatus.Out)
                    {
                        currentPreference.FirstPlaceVotes++;
                        break;
                    }
                }
            }

            var candidatesAboveMinimumThreshold = candidates.Where(x => x.FirstPlaceVotes >= MinimumThreshold);
            if (!candidatesAboveMinimumThreshold.Any())
            {
                var candidatesNotOut = candidates.Where(x => x.Status != CandidateStatus.Out);
                if (candidatesNotOut.Count() <= 2)
                {
                    Console.WriteLine("There is a tie for final winner");
                    candidatesNotOut.ToList().ForEach(x => x.Status = CandidateStatus.Winner);
                    return WinnerSearchResult.Found;
                }

                int bottomCandidateVoteCount = candidatesNotOut.OrderByDescending(x => x.FirstPlaceVotes).Last().FirstPlaceVotes;
                var bottomCandidates = candidates.Where(x => x.FirstPlaceVotes == bottomCandidateVoteCount);
                if (bottomCandidates.Count() > 1)
                {
                    bottomCandidates.ToList().ForEach(x => x.Status = CandidateStatus.NeedsTieBreaking);
                    return WinnerSearchResult.NonFinalTie;
                }

                bottomCandidates.First().Status = CandidateStatus.BeingRemoved;
                return WinnerSearchResult.NotFound;
            }
            else
            {
                candidatesAboveMinimumThreshold.First().Status = CandidateStatus.Winner;
                return WinnerSearchResult.Found;
            }
        }

        public enum WinnerSearchResult
        {
            Found,
            NotFound,
            NonFinalTie
        }
    }
}
