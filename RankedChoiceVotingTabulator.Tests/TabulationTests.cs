namespace RankedChoiceVotingTabulator.Tests
{
    public class TabulationTests
    {
        private Mock<ITabulationService> _mockTabulationService;

        public TabulationTests()
        {
            _mockTabulationService = new Mock<ITabulationService>();
        }

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
            ITabulationService tabulationService = new TabulationService(_mockTabulationService.Object);

            // Act
            tabulationService.Tabulate(viewModel, columnData);

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
            ITabulationService tabulationService = new TabulationService(_mockTabulationService.Object);

            // Act
            tabulationService.Tabulate(viewModel, columnData);

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
            _mockTabulationService.Setup(x => x.DoManualTieBreaker(It.IsAny<HomeViewModel>(), It.IsAny<List<Candidate>>())).Returns(columnData.Candidates.First());
            ITabulationService tabulationService = new TabulationService(_mockTabulationService.Object);

            // Act
            tabulationService.Tabulate(viewModel, columnData);

            // Assert
            Assert.Equal(Candidate.CandidateStatus.Eliminated, columnData.Rounds.First().CandidateVotes.First().Key.Status);
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.Skip(1).First().Key.Status);
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
            ITabulationService tabulationService = new TabulationService(_mockTabulationService.Object);

            // Act
            tabulationService.Tabulate(viewModel, columnData);

            // Assert
            Assert.Equal(2, columnData.Rounds.Count);
            Assert.Single(columnData.Votes.Where(x => x.TopCandidate == null));
            Assert.Equal(Candidate.CandidateStatus.Winner, columnData.Rounds.First().CandidateVotes.First().Key.Status);
        }
    }
}
