using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Income;
using Moq;
using static BudgetPlanner.Application.UseCases.Income.AddIncome;

namespace BudgetPlanner.Application.Tests
{
    public class AddIncomeTests
    {
        private readonly Mock<IIncomeRepository> _mockRepository;
        private readonly AddIncomeHandler _handler;

        public AddIncomeTests()
        {
            _mockRepository = new Mock<IIncomeRepository>();
            _handler = new AddIncomeHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_Success_ReturnsValidId()
        {
            // Arrange
            var command = new AddIncome("Salary", 3000, "Monthly");
            _mockRepository.Setup(repo => repo.SaveIncomeAsync(It.IsAny<Domain.Entities.Income>()))
                           .ReturnsAsync(1); // Simulating successful save

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value);
            _mockRepository.Verify(repo => repo.SaveIncomeAsync(It.IsAny<Domain.Entities.Income>()), Times.Once);
        }

        // Additional tests for failure scenario and input validation...
    }
}