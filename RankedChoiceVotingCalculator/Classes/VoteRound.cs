using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Candidate SearchForWinner()
        {
            List<Candidate> candidates = new List<Candidate>();
            candidates.AddRange(Candidates);

            //count number of first place votes for each candidate
            foreach (Vote vote in Votes)
            {
                for (int loop = 1; loop <= candidates.Count; loop++)
                {
                    Candidate currentPreference = candidates.Where(x => x.Name == vote.OrderPreference[loop]).First();
                    if (!currentPreference.IsOut)
                    {
                        currentPreference.FirstPlaceVotes++;
                        break;
                    }
                }
            }

            var candidatesAboveMinimumThreshold = candidates.Where(x => x.FirstPlaceVotes >= MinimumThreshold);
            if (!candidatesAboveMinimumThreshold.Any())
            { //no winner
                if (candidates.Where(x => !x.IsOut).Count() <= 2)
                {
                    Console.WriteLine("There is a tie for final winner");
                    return null;
                }

                Candidate candidateToRemove = candidates.Where(x => !x.IsOut).OrderByDescending(x => x.FirstPlaceVotes).Last();
                var candidatesTiedForLastPlace = candidates.Where(x => x.FirstPlaceVotes == candidateToRemove.FirstPlaceVotes);
                if (candidatesTiedForLastPlace.Count() > 1)
                {
                    Console.WriteLine("There is a tie in a non-final round:");
                    for (int loop = 0; loop < candidatesTiedForLastPlace.Count(); loop++)
                    {
                        Console.WriteLine($"{loop + 1} - {candidatesTiedForLastPlace.ElementAt(loop).Name}");
                    }
                    Console.Write("Please enter the number of the candidate you want to remove this rounnd: ");
                    int candidateNumberToRemove = 0;
                    while (!int.TryParse(Console.ReadLine(), out candidateNumberToRemove) || !(candidateNumberToRemove > 0) || !(candidateNumberToRemove <= candidatesTiedForLastPlace.Count()))
                    {
                        Console.WriteLine("Please enter a valid number that is shown next to a candidate");
                    }
                    candidateToRemove = candidatesTiedForLastPlace.ElementAt(candidateNumberToRemove - 1);
                }
                candidateToRemove.IsOut = true;

                return candidateToRemove;
            }
            else if (candidatesAboveMinimumThreshold.Count() > 1)
            {
                Console.WriteLine("There are more than two candidates above minimum threshold (this should not be possible)");
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
