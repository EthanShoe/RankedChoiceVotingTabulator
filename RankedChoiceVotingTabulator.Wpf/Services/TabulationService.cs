using System.Drawing;

namespace RankedChoiceVotingTabulator.Wpf.Services
{
    public class TabulationService
    {
        public void Tabulate(HomeViewModel viewModel, ColumnData columnData, IUserInputService userInputService)
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
                    EliminateCandidates(columnData, roundNumber, orderedActiveCandidates.Skip(1).Select(x => x.Key), false);
                    break;
                }

                var lowestVoteCount = orderedActiveCandidates.LastOrDefault().Value;
                var candidatesToBeEliminated = orderedActiveCandidates.Where(x => x.Value == lowestVoteCount).Select(x => x.Key);
                if (candidatesToBeEliminated.Count() > 1 && viewModel.ManualTieBreaking)
                {
                    Candidate? selectedCandidate = userInputService.DoManualTieBreaker(viewModel, candidatesToBeEliminated);

                    if (selectedCandidate != null)
                    {
                        candidatesToBeEliminated = candidatesToBeEliminated.Where(x => x == selectedCandidate);
                    }
                }
                EliminateCandidates(columnData, roundNumber, candidatesToBeEliminated, true);

                if (!columnData.Candidates.Where(x => x.Status != Candidate.CandidateStatus.Eliminated).Any())
                {
                    break;
                }
            }
        }

        private static void EliminateCandidates(ColumnData columnData, int roundNumber, IEnumerable<Candidate> candidatesToBeEliminated, bool calculateTopCandidate)
        {
            foreach (var candidate in candidatesToBeEliminated)
            {
                candidate.Status = Candidate.CandidateStatus.Eliminated;
                candidate.RoundEliminated = roundNumber;

                if (calculateTopCandidate)
                    columnData.Votes.Where(x => x.TopCandidate == candidate).ToList().ForEach(x => x.CalculateTopCandidate());
            }
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
