using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Income;
using BudgetPlanner.Domain.Entities;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Income.UpdateIncome;

namespace BudgetPlanner.Application.Tests;

public class UpdateIncomeTest
{
    private readonly Mock<IIncomeRepository> _mockRepository;
    private readonly UpdateIncomesHandler _handler;

    public UpdateIncomeTest()
    {
        _mockRepository = new Mock<IIncomeRepository>();
        _handler = new UpdateIncomesHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGetOneIncomeAsyncReturnsNull()
    {
        // Arrange
        var command = new UpdateIncome(1, "Test", 1000, "Monthly");
        _mockRepository.Setup(repo => repo.GetOneIncomeAsync(It.IsAny<int>())).ReturnsAsync((Income)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotFetchIncome);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenSaveIncomeAsyncReturnsZero()
    {
        // Arrange
        var command = new UpdateIncome(1, "Test", 1000, "Monthly");
        var income = new Income();
        _mockRepository.Setup(repo => repo.GetOneIncomeAsync(It.IsAny<int>())).ReturnsAsync(income);
        _mockRepository.Setup(repo => repo.SaveIncomeAsync(It.IsAny<Income>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotSaveIncome);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenSaveIncomeAsyncReturnsNonZero()
    {
        // Arrange
        var command = new UpdateIncome(1, "Test", 1000, "Monthly");
        var income = new Income();
        _mockRepository.Setup(repo => repo.GetOneIncomeAsync(It.IsAny<int>())).ReturnsAsync(income);
        _mockRepository.Setup(repo => repo.SaveIncomeAsync(It.IsAny<Income>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }
}