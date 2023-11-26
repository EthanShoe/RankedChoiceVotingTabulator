namespace RankedChoiceVotingTabulator.Tests
{
    public class TabulationTests
    {
        [Fact]
        public void Tabulate_NormalData_ReturnsExpectedData()
        {
            // Arrange
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;", "Candidate1;Candidate2;" },
                3,
                new List<string>() { "Candidate1", "Candidate2" });

            // Act
            TabulationService.Tabulate(columnData);

            // Assert
            Assert.Single(columnData.Rounds);
            Assert.Null(columnData.Rounds.First().CandidateVotes.First().Key.RoundEliminated);
            Assert.Equal(1, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.RoundEliminated);
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
        }

        [Fact]
        public void Tabulate_FinishingTie_BothCandidatesEliminated()
        {
            // Arrange
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;" },
                2,
                new List<string>() { "Candidate1", "Candidate2" });

            // Act
            TabulationService.Tabulate(columnData);

            // Assert
            Assert.Equal(1, columnData.Rounds.First().CandidateVotes.First().Key.RoundEliminated);
            Assert.Equal(1, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.RoundEliminated);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
        }

        [Fact]
        public void Tabulate_OneVoteEndsUpNotCountingTowardFinalCandidate_TopCandidateCanStillWin()
        {
            // Arrange
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate1;Candidate2;", "Candidate1;Candidate2;", "Candidate2;Candidate1;", "Candidate2;Candidate3;", "Candidate3;" },
                6,
                new List<string>() { "Candidate1", "Candidate2", "Candidate3" });

            // Act
            TabulationService.Tabulate(columnData);

            // Assert
            Assert.Equal(2, columnData.Rounds.Count);
            Assert.Single(columnData.Votes.Where(x => x.TopCandidate == null));
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.First().Key.Status);
        }
    }
}
