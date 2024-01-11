using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Income;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Income.DeleteIncome;

namespace BudgetPlanner.Application.Tests;

public class DeleteIncomeTest
{
    private readonly Mock<IIncomeRepository> _mockRepository;
    private readonly DeleteIncomeHandler _handler;

    public DeleteIncomeTest()
    {
        _mockRepository = new Mock<IIncomeRepository>();
        _handler = new DeleteIncomeHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDeleteIncomeAsyncReturnsZero()
    {
        // Arrange
        var command = new DeleteIncome(1);
        _mockRepository.Setup(repo => repo.DeleteIncomeAsync(It.IsAny<int>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotDeleteIncome);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDeleteIncomeAsyncReturnsNonZero()
    {
        // Arrange
        var command = new DeleteIncome(1);
        _mockRepository.Setup(repo => repo.DeleteIncomeAsync(It.IsAny<int>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }
}