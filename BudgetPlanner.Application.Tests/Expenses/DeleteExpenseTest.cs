using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Expenses;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Expenses.DeleteExpense;

namespace BudgetPlanner.Application.Tests;

public class DeleteExpenseTest
{
    private readonly Mock<IExpenseRepository> _mockRepository;
    private readonly DeleteExpensesHandler _handler;

    public DeleteExpenseTest()
    {
        _mockRepository = new Mock<IExpenseRepository>();
        _handler = new DeleteExpensesHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDeleteExpenseAsyncReturnsZero()
    {
        // Arrange
        var command = new DeleteExpense(1);
        _mockRepository.Setup(repo => repo.DeleteExpenseAsync(It.IsAny<int>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotDeleteExpenses);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDeleteExpenseAsyncReturnsNonZero()
    {
        // Arrange
        var command = new DeleteExpense(1);
        _mockRepository.Setup(repo => repo.DeleteExpenseAsync(It.IsAny<int>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }
}