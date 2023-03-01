using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RankedChoiceVotingCalculator
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
                Candidate candidateToRemove = voteRound.SearchForWinner();
                if (candidateToRemove == null)
                {
                    return;
                }
                else
                {
                    Candidates.Where(x => x.Name == candidateToRemove.Name).First().IsOut = true;
                }
            }
        }

        public void PrintResults(ExcelWorkbook excelWorkbook)
        {
            var existingSheet = excelWorkbook.Worksheets.Where(x => x.Name == Name);
            if (existingSheet.Any())
            {
                excelWorkbook.Worksheets.Delete(excelWorkbook.Worksheets.Where(x => x.Name == Name).First().Index);
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
                }
            }
        }

        public List<string> ConvertVoteStringToListOfVotes(string voteString)
        {
            List<string> voteList = voteString.Split(';').ToList();
            //remove last item since it's an empty string
            voteList.RemoveAt(voteList.Count - 1);
            return voteList;
        }
    }
}
