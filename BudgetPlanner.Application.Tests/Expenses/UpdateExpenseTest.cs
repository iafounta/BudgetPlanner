using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Expenses;
using BudgetPlanner.Domain.Entities;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Expenses.UpdateExpense;

namespace BudgetPlanner.Application.Tests;

public class UpdateExpenseTest
{
    private readonly Mock<IExpenseRepository> _mockRepository;
    private readonly UpdateExpensesHandler _handler;

    public UpdateExpenseTest()
    {
        _mockRepository = new Mock<IExpenseRepository>();
        _handler = new UpdateExpensesHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGetOneExpenseAsyncReturnsNull()
    {
        // Arrange
        var command = new UpdateExpense(1, "Test", 1000, "Monthly");
        _mockRepository.Setup(repo => repo.GetOneExpenseAsync(It.IsAny<int>())).ReturnsAsync((Expense)null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotFetchExpenses);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenSaveExpenseAsyncReturnsZero()
    {
        // Arrange
        var command = new UpdateExpense(1, "Test", 1000, "Monthly");
        var expense = new Expense();
        _mockRepository.Setup(repo => repo.GetOneExpenseAsync(It.IsAny<int>())).ReturnsAsync(expense);
        _mockRepository.Setup(repo => repo.SaveExpenseAsync(It.IsAny<Expense>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotSaveExpenses);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenSaveExpenseAsyncReturnsNonZero()
    {
        // Arrange
        var command = new UpdateExpense(1, "Test", 1000, "Monthly");
        var expense = new Expense();
        _mockRepository.Setup(repo => repo.GetOneExpenseAsync(It.IsAny<int>())).ReturnsAsync(expense);
        _mockRepository.Setup(repo => repo.SaveExpenseAsync(It.IsAny<Expense>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
    }
}