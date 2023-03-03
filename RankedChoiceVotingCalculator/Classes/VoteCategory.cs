using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RankedChoiceVotingCalculator.Classes.VoteRound;
using static RankedChoiceVotingCalculator.Classes.Candidate;
using System.Drawing;
using OfficeOpenXml.Style;

namespace RankedChoiceVotingCalculator.Classes
{
    public class VoteCategory
    {
        public VoteCategory(string name, string candidateNamesString, int totalNumberOfVotes)
        {
            Name = name;

            List<string> candidateNames = ConvertVoteStringToListOfVotes(candidateNamesString);
            NumberOfRounds = candidateNames.Count;

            Candidates = new List<Candidate>();
            foreach (string candidateName in candidateNames)
            {
                Candidates.Add(new Candidate(candidateName));
            }
            Votes = new List<Vote>();
            VoteRounds = new List<VoteRound>();

            MinimumThreshold = (int)Math.Floor((decimal)totalNumberOfVotes / 2) + 1;
        }

        public string Name { get; set; }
        public List<Candidate> Candidates { get; set; }
        public List<Vote> Votes { get; set; }
        public List<VoteRound> VoteRounds { get; set; }
        public int NumberOfRounds { get; set; }
        public int MinimumThreshold { get; set; }

        public void AddVotes(string voteString)
        { //vote string referres to the string that MS Forms gives in one cell on the downloaded results
            Votes.Add(new Vote(ConvertVoteStringToListOfVotes(voteString)));
        }

        public void CalculateResults()
        {
            for (int loop = 1; loop <= NumberOfRounds; loop++)
            {
                VoteRound voteRound = new VoteRound(VoteRounds.Count + 1, Candidates, Votes, MinimumThreshold);
                VoteRounds.Add(voteRound);
                switch (voteRound.SearchForWinner())
                {
                    case WinnerSearchResult.Found:
                        return;

                    case WinnerSearchResult.NotFound:
                        Candidates.First(x => x.Name == voteRound.Candidates.First(y => y.Status == CandidateStatus.BeingRemoved).Name).Status = CandidateStatus.Out;
                        break;

                    case WinnerSearchResult.NonFinalTie:
                        //search first vote round to see if the candidates had different scores
                        var currentlyTiedCandidatesFromFirstRound = VoteRounds[0].Candidates.Where(x => voteRound.Candidates.Where(x => x.Status == CandidateStatus.NeedsTieBreaking).Any(y => y.Name == x.Name)).OrderByDescending(x => x.FirstPlaceVotes);
                        var candidatesTiedForLastPlace = currentlyTiedCandidatesFromFirstRound.Where(x => x.FirstPlaceVotes == currentlyTiedCandidatesFromFirstRound.Last().FirstPlaceVotes);
                        if (candidatesTiedForLastPlace.Count() == 1)
                        {
                            voteRound.Candidates.First(x => x.Name == candidatesTiedForLastPlace.First().Name).Status = CandidateStatus.BeingRemoved;
                            Candidates.First(x => x.Name == candidatesTiedForLastPlace.First().Name).Status = CandidateStatus.Out;
                        }
                        else
                        {
                            Console.WriteLine($"There is a tie in a non-final round for {Name}:");
                            for (int loop2 = 0; loop2 < candidatesTiedForLastPlace.Count(); loop2++)
                            {
                                Console.WriteLine($"{loop2 + 1} - {candidatesTiedForLastPlace.ElementAt(loop2).Name}");
                            }
                            Console.Write("Please enter the number of the candidate you want to remove this round: ");
                            int candidateNumberToRemove = 0;
                            while (!int.TryParse(Console.ReadLine(), out candidateNumberToRemove) || !(candidateNumberToRemove > 0) || !(candidateNumberToRemove <= candidatesTiedForLastPlace.Count()))
                            {
                                Console.WriteLine("Please enter a valid number that is shown next to a candidate");
                            }

                            string nameOfCandidateToRemove = candidatesTiedForLastPlace.ElementAt(candidateNumberToRemove - 1).Name;
                            voteRound.Candidates.First(x => x.Name == nameOfCandidateToRemove).Status = CandidateStatus.BeingRemoved;
                            Candidates.First(x => x.Name == nameOfCandidateToRemove).Status = CandidateStatus.Out;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public void PrintResults(ExcelWorkbook excelWorkbook)
        {
            var existingSheet = excelWorkbook.Worksheets.Where(x => x.Name == Name);
            if (existingSheet.Any())
            {
                excelWorkbook.Worksheets.Delete(excelWorkbook.Worksheets.First(x => x.Name == Name).Index);
            }
            ExcelWorksheet newWorksheet = excelWorkbook.Worksheets.Add(Name);

            const int FIRST_ROW = 2;
            for (int rowLoop = 0; rowLoop < Candidates.Count; rowLoop++)
            {
                newWorksheet.Cells[rowLoop + FIRST_ROW, 1].Value = Candidates[rowLoop].Name;
            }

            const int FIRST_COLUMN = 2;
            for (int columnLoop = 0; columnLoop < VoteRounds.Count; columnLoop++)
            {
                VoteRound voteRound = VoteRounds[columnLoop];
                newWorksheet.Cells[1, columnLoop + FIRST_COLUMN].Value = $"Round {voteRound.RoundNumber}";
                for (int rowLoop = 0; rowLoop < voteRound.Candidates.Count; rowLoop++)
                {
                    newWorksheet.Cells[rowLoop + FIRST_ROW, columnLoop + FIRST_COLUMN].Value = voteRound.Candidates[rowLoop].FirstPlaceVotes;
                    if (voteRound.Candidates[rowLoop].Status == CandidateStatus.BeingRemoved)
                    {
                        ColorCell(ref newWorksheet, rowLoop + FIRST_ROW, columnLoop + FIRST_COLUMN, Color.LightGray);
                    }

                    if (voteRound.Candidates[rowLoop].Status == CandidateStatus.Winner)
                    {
                        ColorCell(ref newWorksheet, rowLoop + FIRST_ROW, columnLoop + FIRST_COLUMN, Color.Yellow);
                        ColorCell(ref newWorksheet, rowLoop + FIRST_ROW, 1, Color.Yellow);
                    }
                }
            }

            newWorksheet.Cells[newWorksheet.Dimension.Address].AutoFitColumns();
        }

        public List<string> ConvertVoteStringToListOfVotes(string voteString)
        {
            List<string> voteList = voteString.Split(';').ToList();
            //remove last item since it's an empty string
            voteList.RemoveAt(voteList.Count - 1);
            return voteList;
        }

        public void ColorCell(ref ExcelWorksheet worksheet, int rowNumber, int columnNumber, Color color)
        {
            worksheet.Cells[rowNumber, columnNumber].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowNumber, columnNumber].Style.Fill.BackgroundColor.SetColor(color);
        }
    }
}
