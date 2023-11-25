namespace RankedChoiceVotingTabulator.Tests
{
    public class InputProcessingTests
    {
        [Fact]
        public void GetColumnData_OneValidColumn_ReturnsExpectedData()
        {
            // Arrange
            var service = new InputProcessingService();
            var excelWorksheetMock = new Mock<IExcelWorksheetWrapper>();
            excelWorksheetMock.Setup(x => x.GetRowCount()).Returns(2);
            excelWorksheetMock.Setup(x => x.GetColumnCount()).Returns(6);
            excelWorksheetMock.Setup(x => x.GetColumnCellsByColumnNumber(It.IsAny<int>())).Returns(new List<string> { "Title", "Test1;Test2;" });

            // Act
            var returnData = service.GetColumnData(excelWorksheetMock.Object);

            // Assert
            Assert.Single(returnData);
            Assert.Equal("Title", returnData.First().Title);
            Assert.Equal("1 votes of 1 total", returnData.First().VoteCount);
            Assert.Equal(2, returnData.First().Candidates.Count);
            Assert.Equal(6, returnData.First().ColumnNumber);
        }

        [Fact]
        public void GetColumnData_TwoValidColumns_ReturnsTwoColumns()
        {
            // Arrange
            var service = new InputProcessingService();
            var excelWorksheetMock = new Mock<IExcelWorksheetWrapper>();
            excelWorksheetMock.Setup(x => x.GetRowCount()).Returns(2);
            excelWorksheetMock.Setup(x => x.GetColumnCount()).Returns(7);
            excelWorksheetMock.Setup(x => x.GetColumnCellsByColumnNumber(It.IsAny<int>())).Returns(new List<string> { "Title", "Test1;Test2;" });

            // Act
            var returnData = service.GetColumnData(excelWorksheetMock.Object);

            // Assert
            Assert.Equal(2, returnData.Count);
        }

        [Fact]
        public void GetColumnData_TwoColumnsOneInvalid_ReturnsOneColumn()
        {
            // Arrange
            var service = new InputProcessingService();
            var excelWorksheetMock = new Mock<IExcelWorksheetWrapper>();
            excelWorksheetMock.Setup(x => x.GetRowCount()).Returns(2);
            excelWorksheetMock.Setup(x => x.GetColumnCount()).Returns(7);
            excelWorksheetMock.Setup(x => x.GetColumnCellsByColumnNumber(6)).Returns(new List<string> { "Title", "Test1;Test2;" });
            excelWorksheetMock.Setup(x => x.GetColumnCellsByColumnNumber(7)).Returns(new List<string> { "Title", "Test1" });

            // Act
            var returnData = service.GetColumnData(excelWorksheetMock.Object);

            // Assert
            Assert.Single(returnData);
        }

        [Fact]
        public void GetColumnData_ColumnWithDifferentCandidatesInEntries_ReturnsAllCandidates()
        {
            // Arrange
            var service = new InputProcessingService();
            var excelWorksheetMock = new Mock<IExcelWorksheetWrapper>();
            excelWorksheetMock.Setup(x => x.GetRowCount()).Returns(4);
            excelWorksheetMock.Setup(x => x.GetColumnCount()).Returns(7);
            excelWorksheetMock.Setup(x => x.GetColumnCellsByColumnNumber(It.IsAny<int>())).Returns(new List<string> { "Title", "Test1;Test2;", "Test1;Test2;Test3;", "Test1;Test3;Test4;" });

            // Act
            var returnData = service.GetColumnData(excelWorksheetMock.Object);

            // Assert
            Assert.Equal(4, returnData.First().Candidates.Count);
        }

        [Fact]
        public void GetColumnData_NoColumns_ReturnsEmptyList()
        {
            // Arrange
            var service = new InputProcessingService();
            var excelWorksheetMock = new Mock<IExcelWorksheetWrapper>();
            excelWorksheetMock.Setup(x => x.GetRowCount()).Returns(2);
            excelWorksheetMock.Setup(x => x.GetColumnCount()).Returns(5);

            // Act
            var returnData = service.GetColumnData(excelWorksheetMock.Object);

            // Assert
            Assert.Empty(returnData);
        }
    }
}