using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Income;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Income.AddIncome;

namespace BudgetPlanner.Application.Tests;

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
    public async Task Handle_Should_ReturnFailure_WhenSaveIncomeAsyncReturnsZero()
    {
        // Arrange
        var command = new AddIncome("Test", 1000, "Monthly");
        _mockRepository.Setup(repo => repo.SaveIncomeAsync(It.IsAny<Domain.Entities.Income>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotSaveIncome);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenSaveIncomeAsyncReturnsValidId()
    {
        // Arrange
        var command = new AddIncome("Test", 3000, "Monthly");
        _mockRepository.Setup(repo => repo.SaveIncomeAsync(It.IsAny<Domain.Entities.Income>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}