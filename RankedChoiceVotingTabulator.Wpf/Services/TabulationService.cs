using System.Drawing;

namespace RankedChoiceVotingTabulator.Wpf.Services
{
    public class TabulationService
    {
        public static void Tabulate(HomeViewModel viewModel, ColumnData columnData)
        {
            for (int roundNumber = 1; roundNumber <= columnData.Candidates.Count; roundNumber++)
            {
                var round = new Round(roundNumber);
                columnData.Rounds.Add(round);
                foreach (var candidate in columnData.Candidates)
                {
                    round.CandidateVotes.Add(candidate, columnData.Votes.Count(x => x.TopCandidate == candidate));
                }

                var orderedActiveCandidates = round.CandidateVotes.Where(x => x.Key.Status != Candidate.CandidateStatus.Eliminated).OrderByDescending(x => x.Value);
                if (orderedActiveCandidates.FirstOrDefault().Value >= Math.Floor((decimal)columnData.Votes.Where(x => x.TopCandidate != null).Count() / 2) + 1)
                {
                    orderedActiveCandidates.FirstOrDefault().Key.Status = Candidate.CandidateStatus.Winner;
                    orderedActiveCandidates.Skip(1).ToList().ForEach(x => EliminateCandidate(roundNumber, x.Key));
                    break;
                }

                var lowestVoteCount = orderedActiveCandidates.LastOrDefault().Value;
                var candidatesToBeEliminated = orderedActiveCandidates.Where(x => x.Value == lowestVoteCount).Select(x => x.Key);
                if (viewModel.ManualTieBreaking)
                {

                }
                else
                {
                    foreach (var candidate in candidatesToBeEliminated)
                    {
                        EliminateCandidate(roundNumber, candidate);
                        columnData.Votes.Where(x => x.TopCandidate == candidate).ToList().ForEach(x => x.CalculateTopCandidate());
                    }
                }

                if (!columnData.Candidates.Where(x => x.Status != Candidate.CandidateStatus.Eliminated).Any())
                {
                    break;
                }
            }
        }

        private static void EliminateCandidate(int roundNumber, Candidate candidate)
        {
            candidate.Status = Candidate.CandidateStatus.Eliminated;
            candidate.RoundEliminated = roundNumber;
        }

        public static void WriteResults(ColumnData columnData, ExcelWorksheetWrapper worksheet)
        {
            worksheet.SetCellsRow(1, 2, columnData.Rounds.Select(x => $"Round {x.Number}").ToList());

            foreach (var candidate in columnData.Candidates)
            {
                var candidateRow = columnData.Candidates.IndexOf(candidate) + 2;
                worksheet.SetCell(candidateRow, 1, candidate.Name);
                var votesEachRound = columnData.Rounds.SelectMany(x => x.CandidateVotes.Where(y => y.Key == candidate).Select(y => y.Value)).ToList();
                worksheet.SetCellsRow(candidateRow, 2, votesEachRound);
                if (candidate.RoundEliminated != null)
                    worksheet.ColorCell(candidateRow, (int)(candidate.RoundEliminated + 1), Color.LightGray);
                else
                    worksheet.ColorCell(candidateRow, columnData.Rounds.Count + 1, Color.Yellow);
            }
            worksheet.SetAllColumnsAutoWidth();
        }
    }
}
