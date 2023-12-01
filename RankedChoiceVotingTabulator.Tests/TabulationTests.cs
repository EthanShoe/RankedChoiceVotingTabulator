namespace RankedChoiceVotingTabulator.Tests
{
    public class TabulationTests
    {
        [Fact]
        public void Tabulate_NormalData_ReturnsExpectedData()
        {
            // Arrange
            HomeViewModel viewModel = new HomeViewModel()
            {
                ManualTieBreaking = false
            };
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;", "Candidate1;Candidate2;" },
                3,
                new List<string>() { "Candidate1", "Candidate2" });

            // Act
            new TabulationService().Tabulate(viewModel, columnData, new UserInputService());

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
            HomeViewModel viewModel = new HomeViewModel()
            {
                ManualTieBreaking = false
            };
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;" },
                2,
                new List<string>() { "Candidate1", "Candidate2" });

            // Act
            new TabulationService().Tabulate(viewModel, columnData, new UserInputService());

            // Assert
            Assert.Equal(1, columnData.Rounds.First().CandidateVotes.First().Key.RoundEliminated);
            Assert.Equal(1, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.RoundEliminated);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
        }

        [Fact]
        public void Tabulate_FinishingTieWithTieBreaking_OneCandidateIsSelectedAndEliminated()
        {
            // Arrange
            HomeViewModel viewModel = new HomeViewModel()
            {
                ManualTieBreaking = true
            };
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;" },
                2,
                new List<string>() { "Candidate1", "Candidate2" });
            var userInputServiceMock = new Mock<IUserInputService>();
            userInputServiceMock.Setup(x => x.DoManualTieBreaker(It.IsAny<HomeViewModel>(), It.IsAny<IEnumerable<Candidate>>())).Returns(columnData.Candidates.First());

            // Act
            new TabulationService().Tabulate(viewModel, columnData, userInputServiceMock.Object);

            // Assert
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
        }

        [Fact]
        public void Tabulate_FinishingTieWithTieBreaking_NoCandidateIsSelectedAndBothAreEliminated()
        {
            // Arrange
            HomeViewModel viewModel = new HomeViewModel()
            {
                ManualTieBreaking = true
            };
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate2;Candidate1;" },
                2,
                new List<string>() { "Candidate1", "Candidate2" });
            var userInputServiceMock = new Mock<IUserInputService>();
            userInputServiceMock.Setup(x => x.DoManualTieBreaker(It.IsAny<HomeViewModel>(), It.IsAny<IEnumerable<Candidate>>())).Returns(null as Candidate);

            // Act
            new TabulationService().Tabulate(viewModel, columnData, userInputServiceMock.Object);

            // Assert
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
        }

        [Fact]
        public void Tabulate_OneVoteEndsUpNotCountingTowardFinalCandidate_TopCandidateCanStillWin()
        {
            // Arrange
            HomeViewModel viewModel = new HomeViewModel()
            {
                ManualTieBreaking = false
            };
            ColumnData columnData = new ColumnData(
                "Title",
                6,
                new List<string>() { "Candidate1;Candidate2;", "Candidate1;Candidate2;", "Candidate1;Candidate2;", "Candidate2;Candidate1;", "Candidate2;Candidate3;", "Candidate3;" },
                6,
                new List<string>() { "Candidate1", "Candidate2", "Candidate3" });

            // Act
            new TabulationService().Tabulate(viewModel, columnData, new UserInputService());

            // Assert
            Assert.Equal(2, columnData.Rounds.Count);
            Assert.Single(columnData.Votes.Where(x => x.TopCandidate == null));
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.First().Key.Status);
        }
    }
}
